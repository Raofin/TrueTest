'use client'

import React, { useCallback, useEffect, useMemo, useState } from 'react'
import type { Selection } from '@heroui/react'
import {
  Table,
  TableHeader,
  TableColumn,
  TableBody,
  TableRow,
  TableCell,
  Input,
  Button,
  Pagination,
} from '@heroui/react'
import SearchIcon from '@/components/ui/search_icon/search-icon'
import { AxiosError } from 'axios'
import api from '@/utils/api'
import FormattedDate from '@/components/format-date-time'
type User = {
  isActive: boolean
  profile: [] | null
  updatedAt: string | null
  createdAt: string
  accountId: string
  username: string
  email: string
  roles: string
  action?: string
}

const columns = [
  { label: 'Username', key: 'username' },
  { label: 'Email', key: 'email' },
  { label: 'Role', key: 'roles' },
  { label: 'Created At', key: 'createdAt' },
]

export default function Component() {
  const [filterValue, setFilterValue] = useState('')
  const rowsPerPage = 10
  const [page, setPage] = useState(1)
  const hasSearchFilter = Boolean(filterValue)
  const [allUsers, setAllUsers] = useState<User[]>([])
  const [error, setError] = useState('')
  const [selectedKeys, setSelectedKeys] = useState<Selection>(new Set())
  const [invitedEmail, setInvitedEmail] = useState('')

  useEffect(() => {
    const ManageUser = async () => {
      try {
        const response = await api.get('Account/All')
        if (response.status === 200) {
          setAllUsers(response.data)
        }
      } catch (err) {
        const error = err as AxiosError
        setError(error.message)
      }
    }
    ManageUser()
  }, [])
  const filteredItems = useMemo(() => {
    let filteredUsers = [...allUsers]
    if (hasSearchFilter) {
      filteredUsers = filteredUsers.filter((user) => user.email.toLowerCase().includes(filterValue.toLowerCase()))
    }
    return filteredUsers
  }, [filterValue, hasSearchFilter, allUsers])

  const pages = Math.ceil(filteredItems.length / rowsPerPage)

  const items = useMemo(() => {
    const start = (page - 1) * rowsPerPage
    const end = start + rowsPerPage
    return filteredItems.slice(start, end)
  }, [page, filteredItems, rowsPerPage])

  const onNextPage = useCallback(() => {
    if (page < pages) {
      setPage(page + 1)
    }
  }, [page, pages])

  const onPreviousPage = useCallback(() => {
    if (page > 1) {
      setPage(page - 1)
    }
  }, [page])

  const renderCell = useCallback((user: User, columnKey: React.Key) => {
    const cellValue = user[columnKey as keyof User]
    if (columnKey === 'createdAt') return <FormattedDate date={cellValue as string} />
    else if (columnKey === 'roles') {
      if (Array.isArray(cellValue)) {
        return cellValue.map((curr) => <div key={curr}>{curr}</div>)
      }
    } else return cellValue as React.ReactNode
  }, [])

  const onSearchChange = useCallback((value?: string) => {
    if (value) {
      setFilterValue(value)
      setPage(1)
    } else {
      setFilterValue('')
    }
  }, [])

  const onClear = useCallback(() => {
    setFilterValue('')
    setPage(1)
  }, [])

  const handleMakeAdmin = useCallback(async () => {
    const selectedEmail = Array.from(selectedKeys)
    const selectedUsers = allUsers.filter((e) => selectedEmail.includes(e.email))
    const adminUsers = selectedUsers.map((e) => ({ accountId: e.accountId }))
    try {
      for (const user of adminUsers) {
        await api.post('Account/MakeAdmin', user)
      }
      setAllUsers((prev) =>
        prev.map((e) => (selectedEmail.includes(e.email) ? { ...e, roles: `${e.roles},admin` } : e))
      )
    } catch (error) {
      console.error('Error :', error)
    }
    setSelectedKeys(new Set())
  }, [selectedKeys, allUsers])
  const handleInvitation = useCallback(async () => {
    try {
      await api.post('Account/SendAdminInvite', { email: invitedEmail })
    } catch (error) {
      console.error('Error :', error)
    }
    setInvitedEmail('')
  }, [invitedEmail])

  const topContent = useMemo(
    () => (
      <div className="flex w-full justify-between px-5 my-3">
        <p>User List</p>
        <Input
          isClearable
          className="w-[400px] bg-[#eeeef0] dark:[#71717a] rounded-2xl"
          placeholder="Search"
          startContent={<SearchIcon />}
          value={filterValue}
          onClear={onClear}
          onValueChange={onSearchChange}
        />
      </div>
    ),
    [filterValue, onClear, onSearchChange]
  )

  return (
    <div className="h-screen flex flex-col justify-between">
      <h2 className="text-2xl font-bold text-center my-5">Add Admins</h2>
      <div className="mx-12 flex h-screen flex-col justify-between  rounded-xl bg-white dark:bg-[#18181b]">
        <div className="flex gap-3 w-full p-3 mt-5">
          <Input
            isClearable
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-2xl"
            placeholder="Email Address"
            onClear={onClear}
            value={invitedEmail}
            onChange={(e) => setInvitedEmail(e.target.value)}
          />
          <Button color="primary" onPress={handleInvitation}>
            Send Invitation
          </Button>
        </div>
        {error && <p className="text-red-500 text-xl">{error}</p>}
        <Table
          aria-label="Example table with custom cells, pagination, and sorting"
          topContent={topContent}
          topContentPlacement="outside"
          classNames={{
            wrapper: ' overflow-y-auto',
          }}
          selectedKeys={selectedKeys}
          selectionMode="multiple"
          onSelectionChange={setSelectedKeys}
        >
          <TableHeader>
            {columns.map((column) => (
              <TableColumn key={column.key} align={'center'} className={'font-semibold'}>
                {column.label}
              </TableColumn>
            ))}
          </TableHeader>
          <TableBody emptyContent="No admin found" className={items.length === 0 ? 'min-h-[70vh]' : 'min-h-[auto]'}>
            {items.map((item) => (
              <TableRow key={item.email}>
                {columns.map((column) => (
                  <TableCell key={column.key}>{renderCell(item, column.key)}</TableCell>
                ))}
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <div className="py-2 px-2 flex justify-between items-center">
          <span className="w-[30%] text-small text-default-400">
            Page {page} out of {pages}
          </span>
          <Pagination isCompact showControls showShadow color="primary" page={page} total={pages} onChange={setPage} />
          <div className="hidden sm:flex w-[30%] justify-end gap-2">
            <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onPreviousPage}>
              Previous
            </Button>
            <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onNextPage}>
              Next
            </Button>
            <Button color="primary" size="sm" onPress={handleMakeAdmin}>
              Make Admin
            </Button>
          </div>
        </div>
      </div>
    </div>
  )
}
