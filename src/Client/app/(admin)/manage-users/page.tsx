'use client'

import React, { useCallback, useEffect, useMemo, useState } from 'react'
import { MdDelete } from 'react-icons/md'
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
  Select,
  SelectItem,
} from '@heroui/react'
import SearchIcon from '@/components/ui/search-icon'
import PaginationButtons from '@/components/ui/pagination-button'
import CommonModal from '@/components/ui/Modal/edit-delete-modal'
import api from '@/utils/api'
import { AxiosError } from 'axios'
import FormattedDate from '@/components/format-date-time'
// import StatusIcon from '@/components/ui/status-icon'
import handleDelete from '@/components/handleDelete'
import handleStatus from '@/components/handleStatus'

const columns = [
  { label: 'Username', key: 'username' },
  { label: 'Email', key: 'email' },
  { label: 'Role', key: 'roles' },
  { label: 'Created At', key: 'createdAt' },
  { label: 'Action', key: 'action' },
]

type User = {
  username: string
  email: string
  roles: string[]
  createdAt: string
  action?: string
  accountId: string
}

export default function Component() {
  const [filterValue, setFilterValue] = useState('')
  const [roleFilter, setRoleFilter] = useState('')
  const [rowsPerPage, setRowsPerPage] = useState(10)
  const [page, setPage] = useState(1)
  const [isActiveModalOpen, setIsActiveModalOpen] = useState(false)
  const [usersData, setUserData] = useState<User[]>([])
  const [error, setError] = useState('')
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [selectedUser, setSelectedUser] = useState('')
  const [status, setStatus] = useState<boolean>(true)
  const hasSearchFilter = Boolean(filterValue)
  const hasRoleFilter = Boolean(roleFilter)

  useEffect(() => {
    const ManageUser = async () => {
      try {
        const response = await api.get<{ page: number; accounts: User[] }>(
          `/Account?pageIndex=${page}&pageSize=${rowsPerPage}${roleFilter ? `&role=${roleFilter}` : ''}`
        )
        if (response.status === 200) {
          setUserData(response.data.accounts)
        }
      } catch (err) {
        const axiosError = err as AxiosError
        setError(axiosError.message)
      }
    }
    ManageUser()
  }, [page, roleFilter, rowsPerPage])

  const filteredItems = useMemo(() => {
    let filteredUsers = [...usersData]
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
  }, [filterValue, hasSearchFilter, usersData, roleFilter, hasRoleFilter])

  const pages = useMemo(() => {
    console.log(usersData.length)
    const calculatedPages = Math.ceil(usersData.length / rowsPerPage)
    return calculatedPages
  }, [rowsPerPage, usersData.length])

  const items = useMemo(() => {
    const start = (page - 1) * rowsPerPage
    const end = start + rowsPerPage
    return filteredItems.slice(start, end)
  }, [page, filteredItems, rowsPerPage])

  const renderCell = useCallback((user: User, columnKey: React.Key) => {
    const cellValue = user[columnKey as keyof User]
    if (columnKey === 'roles') {
      if (Array.isArray(cellValue) && cellValue?.length > 1) {
        return cellValue.map((e) => (
          <p className="w-full" key={e}>
            {e}
          </p>
        ))
      }
    }
    if (columnKey === 'action') {
      return (
        <div className="flex gap-4 ml-16">
          <button
            aria-label="Change Status"
            onClick={() => {
              setSelectedUser(user.accountId)
              setIsActiveModalOpen(true)
            }}>
          {/* <StatusIcon /> */}
          {status?"Active":"Deactive"}
          </button>
          <button 
            aria-label="Delete User"
            onClick={() => {
              setSelectedUser(user.accountId)
              setIsDeleteModalOpen(true)
            }}
          >
            <MdDelete className={'text-xl'} />
          </button>
        </div>
      )
    } else if (columnKey === 'createdAt') {
      return <FormattedDate date={cellValue as string} />
    } else {
      return cellValue as React.ReactNode
    }
  }, [status])

  const onSearchChange = useCallback((value?: string) => {
    setFilterValue(value || '')
    setPage(1)
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

  const topContent = useMemo(
    () => (
      <div className="flex gap-5 p-3 w-full flex-col items-center mt-5">
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
        <div className="w-full flex justify-between items-center gap-4">
          <h2 className="ml-3">Users List</h2>
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
    [filterValue, onClear, onSearchChange, roleFilter, rowsPerPage, onRoleChange]
  )

  return (
    <div className="flex flex-col justify-between">
      <h2 className="text-2xl font-bold my-5 text-center flex justify-center"> Manage Users</h2>
      <div className="mx-12 flex min-h-screen flex-col justify-between rounded-xl bg-white dark:bg-[#18181b]">
        {error && <p className="text-red-500 text-xl">{error}</p>}
        <Table
          aria-label="Example table with custom cells, pagination"
          topContent={topContent}
          topContentPlacement="outside"
          selectionMode="multiple"
          classNames={{
            wrapper: ' ',
            table: 'w-full ',
          }}
        >
          <TableHeader>
            {columns.map((column) => (
              <TableColumn key={column.key} align={'center'} className={'font-semibold'}>
                {column.label}
              </TableColumn>
            ))}
          </TableHeader>
          <TableBody emptyContent="No user found" className={items.length === 0 ? 'min-h-[70vh]' : 'min-h-[auto]'}>
            {items.map((item) => (
              <TableRow key={item.email}>
                {columns.map((column) => (
                  <TableCell key={column.key}>{renderCell(item, column.key)}</TableCell>
                ))}
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <div className="py-4 px-2 flex justify-between items-center">
          <Pagination isCompact showControls showShadow color="primary" page={page} total={pages} onChange={setPage} />
          <span className=" text-small text-default-400">
            Page {page} out of {pages}
          </span>
          <div className="flex gap-2">
            <PaginationButtons
              currentIndex={page}
              totalItems={pages}
              onPrevious={() => setPage(Math.max(1, page - 1))}
              onNext={() => setPage(Math.min(pages, page + 1))}
            />
            <Button color="primary" size="sm">
              Export
            </Button>
          </div>
        </div>
      </div>
      <CommonModal
        isOpen={isActiveModalOpen}
        onClose={() => {
          setIsActiveModalOpen(false)
          setSelectedUser('')
        }}
        title="Status Confirmation"
        content={`Do you want to deactive this record`}
        confirmButtonText="Deactive"
        onConfirm={async () => {
          await handleStatus(selectedUser,setStatus)
          setIsActiveModalOpen(false)
          setSelectedUser('')
        }}
      />
      <CommonModal
        isOpen={isDeleteModalOpen}
        onClose={() => {
          setIsDeleteModalOpen(false)
          setSelectedUser('')
        }}
        title="Delete Confirmation"
        content={`Do you want to delete this record?`}
        confirmButtonText="Delete"
        onConfirm={async () => {
          const success = await handleDelete(selectedUser)
          if (success) {
            setUserData((prevUsers) => prevUsers.filter((user) => user.accountId !== selectedUser))
          }
          setIsDeleteModalOpen(false)
          setSelectedUser('')
        }}
      />
    </div>
  )
}
