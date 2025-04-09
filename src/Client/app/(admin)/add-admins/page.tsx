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
  SelectItem,
  Select,
} from '@heroui/react'
import SearchIcon from '@/components/ui/search-icon'
import api from '@/utils/api'
import FormattedDate from '@/components/format-date-time'
import isValidEmail from '@/components/check-valid-email'
import toast from 'react-hot-toast'

type User = {
  isActive: boolean
  profile: [] | null
  updatedAt: string | null
  createdAt: string
  accountId: string
  username: string
  email: string
  roles: string[]
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
  const [rowsPerPage, setRowsPerPage] = useState(10)
  const [roleFilter, setRoleFilter] = useState('')
  const [page, setPage] = useState(1)
  const hasSearchFilter = Boolean(filterValue)
  const hasRoleFilter = Boolean(roleFilter)
  const [allUsers, setAllUsers] = useState<User[]>([])
  const [error, setError] = useState('')
  const [selectedKeys, setSelectedKeys] = useState<Selection>(new Set())
  const [invitedEmails, setInvitedEmails] = useState('')

  useEffect(() => {
    const ManageUser = async () => {
      try {
        const response = await api.get<{ page: number; accounts: User[] }>(
          `/Account?pageIndex=${page}&pageSize=${rowsPerPage}${roleFilter ? `&role=${roleFilter}` : ''}`
        )
        if (response.status === 200) {
          setAllUsers(response.data.accounts)
        }
      } catch {
        setError('An error has occured.')
      }
    }
    ManageUser()
  }, [page, roleFilter, rowsPerPage])

  const filteredItems = useMemo(() => {
    let filteredUsers = [...allUsers]
    if (hasSearchFilter) {
      filteredUsers = filteredUsers.filter(
        (user) =>
          user.email?.toLowerCase().includes(filterValue.toLowerCase()) ||
          user.username?.toLowerCase().includes(filterValue.toLowerCase())
      )
    }
    if (hasRoleFilter) {
      filteredUsers = filteredUsers.filter((user) =>
        user.roles?.some((role) => role.toLowerCase() === roleFilter.toLowerCase())
      )
    }
    return filteredUsers
  }, [filterValue, hasSearchFilter, allUsers, roleFilter, hasRoleFilter])

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
    setRoleFilter('')
    setPage(1)
  }, [])

  const onRoleChange = useCallback((value: string) => {
    setRoleFilter(value)
    setPage(1)
  }, [])
  const handleMakeAdmin = useCallback(async () => {
    const selectedEmail = Array.from(selectedKeys)
    if(selectedEmail.length==0){
      alert("please select an account to make admin.")
    }
    const selectedUsers = allUsers.filter((e) => selectedEmail.includes(e.email))

    try {
      const response = await api.patch('/Account/MakeAdmin', {
        accountIds: selectedUsers.map((e) => e.accountId),
      })

      if (response.status === 200) {
        setAllUsers((prev) =>
          prev.map((e) =>
            selectedEmail.includes(e.email) ? { ...e, roles: Array.from(new Set([...e.roles, 'Admin'])) } : e
          )
        )
        setSelectedKeys(new Set())
        toast.success('Selected users have been made admins successfully!')
      }
    } catch  {
      toast.error('Failed to make admin. Please try again.')
    }
  }, [selectedKeys, allUsers])

  const handleInvitation = useCallback(async () => {
    const emailsToSend = invitedEmails
      .split(',')
      .map((email) => email.trim())
      .filter(Boolean)

    const invalidEmails = emailsToSend.filter((email) => !isValidEmail(email))

    if (invalidEmails.length > 0) {
      alert(`Emails are invalid`)
      return
    }

    if (emailsToSend.length === 0) {
      alert('Please enter at least one valid email address')
      return
    }

    try {
      const response = await api.post('/Account/SendAdminInvite', {
        email: emailsToSend,
      })
      if (response.status === 200) {
        toast.success('Invitations sent successfully!')
        setInvitedEmails('')
      }
    } catch {
      toast.error('Failed to send invitations. Please try again.')
    }
  }, [invitedEmails])

  const topContent = useMemo(
    () => (
      <div className="flex gap-3 p-3 w-full flex-col items-center mt-5">
        <div className="w-full flex justify-end gap-3">
          <Select
            label="Filter by Role"
            className="w-[150px]"
            items={[
              { key: '', value: '', label: 'All Roles' },
              { key: 'Admin', value: 'Admin', label: 'Admin' },
              { key: 'Candidate', value: 'Candidate', label: 'Candidate' },
            ]}
            selectedKeys={[roleFilter]}
            onSelectionChange={(keys) => {
              for (const key of keys) {
                onRoleChange(key as string)
                break
              }
              if (!keys) {
                onRoleChange('')
              }
            }}
          >
            {(item) => <SelectItem key={item.key}>{item.label}</SelectItem>}
          </Select>
          <Select
            label="Rows per page"
            className="w-[150px]"
            selectedKeys={[String(rowsPerPage)]}
            onSelectionChange={(keys) => {
              for (const key of keys) {
                const selectedValue = key as string
                if (selectedValue) {
                  setRowsPerPage(Number(selectedValue))
                  break
                }
              }
            }}
            items={[
              { key: '5', value: '5', label: '5' },
              { key: '10', value: '10', label: '10' },
              { key: '15', value: '15', label: '15' },
              { key: '20', value: '20', label: '20' },
              { key: '50', value: '50', label: '50' },
            ]}
          >
            {(item) => <SelectItem key={item.key}>{item.label}</SelectItem>}
          </Select>
        </div>
        <div className="flex w-full justify-between ">
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
      </div>
    ),
    [filterValue, onClear, onRoleChange, onSearchChange, roleFilter, rowsPerPage]
  )

  return (
    <div className=" flex flex-col justify-between">
      <h2 className="text-2xl font-bold text-center my-5">Add Admins</h2>
      <div className="mx-12 flex  flex-col justify-between rounded-xl bg-white dark:bg-[#18181b]">
        <div className="flex gap-3 w-full p-3 mt-5">
          <Input
            isClearable
            className="bg-[#eeeef0] dark:bg-[#27272a] rounded-2xl"
            placeholder="Email Addresses"
            onClear={onClear}
            value={invitedEmails}
            onChange={(e) => setInvitedEmails(e.target.value)}
          />
          <Button color="primary" onPress={handleInvitation}>
            Send Invitations
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
