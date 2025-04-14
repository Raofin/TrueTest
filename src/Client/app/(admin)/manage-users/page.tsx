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
} from '@heroui/react'
import SearchIcon from '@/components/ui/search-icon'
import PaginationButtons from '@/components/ui/pagination-button'
import CommonModal from '@/components/ui/Modal/edit-delete-modal'
import api from '@/utils/api'
import { AxiosError } from 'axios'
import {FormatDatewithTime} from '@/components/format-date-time'
import handleDelete from '@/components/handleDelete'
import handleStatus from '@/components/handleStatus'
import RoleFilter from '@/components/role-filter'
import Paginate from '@/components/pagination'

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
  isActive: boolean
  accountId: string
}
type ApiResponse = {
  page: {
    index: number
    size: number
    totalCount: number
    totalPages: number
    hasNext: boolean
    hasPrevious: boolean
  }
  accounts: User[]
}
export default function Component() {
  const [filterValue, setFilterValue] = useState('')
  const [rowsPerPage, setRowsPerPage] = useState(10)
  const [roleFilter, setRoleFilter] = useState('')
  const [searchTerm, setSearchTerm] = useState('')
  const [totalPages, setTotalPages] = useState(1)
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
        const response = await api.get<ApiResponse>(
          `/Account?pageIndex=${page}&pageSize=${rowsPerPage}${roleFilter ? `&role=${roleFilter}` : ''}${
            searchTerm ? `&searchTerm=${searchTerm}` : ''
          }`
        )
        if (response.status === 200) {
          setUserData(response.data.accounts)
          setTotalPages(response.data.page.totalPages || 1)
        }
      } catch (err) {
        const axiosError = err as AxiosError
        setError(axiosError.message)
      }
    }
    ManageUser()
  }, [page, roleFilter, rowsPerPage, searchTerm])

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
        <div className="flex w-[100px] gap-5 justify-between ml-6">
          <button
            aria-label="Change Status"
            onClick={() => {
              setSelectedUser(user.accountId)
              setIsActiveModalOpen(true)
            }}
          >
            {user.isActive ? 'Deactive'  : 'Active'}
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
      return FormatDatewithTime(cellValue as string)
    } else {
      return cellValue as React.ReactNode
    }
  }, [])

  const onSearchChange = useCallback((value?: string) => {
    setSearchTerm(value || '')
    setPage(1)
  }, [])

  const onClear = useCallback(() => {
    setSearchTerm('')
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
          <RoleFilter roleFilter={roleFilter} onRoleChange={onRoleChange} />
          <Paginate rowsPerPage={rowsPerPage} setRowsPerPage={setRowsPerPage} />
        </div>
        <div className="w-full flex justify-between items-center gap-4">
          <h2 className="ml-3">Users List</h2>
          <Input
            isClearable
            className="w-[400px] bg-[#eeeef0] dark:[#71717a] rounded-2xl"
            placeholder="Search"
            startContent={<SearchIcon />}
            value={searchTerm}
            onClear={onClear}
            onValueChange={onSearchChange}
          />
        </div>
      </div>
    ),
    [roleFilter, onRoleChange, rowsPerPage, searchTerm, onClear, onSearchChange]
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
          <TableBody
            emptyContent="No user found"
            className={filteredItems.length === 0 ? 'min-h-[70vh]' : 'min-h-[auto]'}
          >
            {filteredItems.map((item) => (
              <TableRow key={item.accountId}>
                {columns.map((column) => (
                  <TableCell key={column.key}>{renderCell(item, column.key)}</TableCell>
                ))}
              </TableRow>
            ))}
          </TableBody>
        </Table>
        <div className="py-4 px-2 flex justify-between items-center">
          <Pagination
            isCompact
            showControls
            showShadow
            color="primary"
            page={page}
            total={totalPages}
            onChange={setPage}
          />{' '}
          <span className=" text-small text-default-400">
            Page {page} out of {totalPages}
          </span>
          <div className="flex gap-2">
            <PaginationButtons
              currentIndex={page}
              totalItems={totalPages}
              onPrevious={() => setPage(Math.max(1, page - 1))}
              onNext={() => setPage(Math.min(totalPages, page + 1))}
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
        content={`Do you want to ${status ? 'Deactive' : 'Active'} this record`}
        confirmButtonText={`${status ? 'Deactive' : 'Active'}`}
        onConfirm={async () => {
         const resp= await handleStatus(selectedUser, setStatus)
           if(resp){
            setUserData(prevUsers => 
              prevUsers.map(user => 
                user.accountId === selectedUser 
                  ? { ...user, isActive: !status } 
                  : user
              )
            );
          setIsActiveModalOpen(false)
          setSelectedUser('')
           }
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
