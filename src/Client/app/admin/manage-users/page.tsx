'use client'

import React, { useCallback, useMemo, useState } from 'react'
import { MdDelete } from 'react-icons/md'
import { FaEdit } from 'react-icons/fa'
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
import SearchIcon from 'app/components/table/search_icon/page'
import PaginationButtons from 'app/components/ui/pagination-button'
import CommonModal from 'app/components/ui/Modal/edit-delete-modal'

const columns = [
  { label: 'Username', key: 'username' },
  { label: 'Email', key: 'email' },
  { label: 'Role', key: 'role' },
  { label: 'Created At', key: 'createdAt' },
  { label: 'Action', key: 'action' },
]

const usersData = [
  { key: '1', username: 'Eve', email: 'eve@example.com', role: 'User', createdAt: new Date('2025-05-30T18:50:00') },
  {
    key: '2',
    username: 'Alice',
    email: 'alice@example.com',
    role: 'Admin',
    createdAt: new Date('2025-01-15T10:15:00'),
  },
  {
    key: '3',
    username: 'Bob',
    email: 'bob@example.com',
    role: 'Moderator',
    createdAt: new Date('2025-03-22T16:45:00'),
  },
  {
    key: '4',
    username: 'Charlie',
    email: 'charlie@example.com',
    role: 'Candidate',
    createdAt: new Date('2025-02-10T09:30:00'),
  },
  {
    key: '5',
    username: 'David',
    email: 'david@example.com',
    role: 'Editor',
    createdAt: new Date('2025-04-05T13:20:00'),
  },
  { key: '6', username: 'Eve', email: 'eve12@example.com', role: 'User', createdAt: new Date('2025-05-30T18:50:00') },
]

type User = {
  username: string
  email: string
  role: string
  createdAt: Date
  action?: string
}

export default function Component() {
  const [filterValue, setFilterValue] = useState('')
  const rowsPerPage = 10
  const [page, setPage] = useState(1)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const hasSearchFilter = Boolean(filterValue)

  const filteredItems = useMemo(() => {
    let filteredUsers = [...usersData]
    if (hasSearchFilter) {
      filteredUsers = filteredUsers.filter(
        (user) =>
          user.email.toLowerCase().includes(filterValue.toLowerCase()) ||
          user.username.toLowerCase().includes(filterValue.toLowerCase())
      )
    }
    return filteredUsers
  }, [filterValue, hasSearchFilter])

  const pages = Math.ceil(filteredItems.length / rowsPerPage)

  const items = useMemo(() => {
    const start = (page - 1) * rowsPerPage
    const end = start + rowsPerPage
    return filteredItems.slice(start, end)
  }, [page, filteredItems, rowsPerPage])

  const formatDate = useCallback((date: Date) => {
    return date.toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: 'numeric',
      minute: 'numeric',
    })
  }, [])

  const renderCell = useCallback(
    (user: User, columnKey: React.Key) => {
      const cellValue = user[columnKey as keyof User]
      switch (columnKey) {
        case 'action':
          return (
            <div className="flex gap-4 ml-16">
              <button aria-label="Edit User" onClick={() => setIsEditModalOpen(true)}>
                <FaEdit className={'text-xl'} />
              </button>
              <button aria-label="Delete User" onClick={() => setIsDeleteModalOpen(true)}>
                <MdDelete className={'text-xl'} />
              </button>
            </div>
          )
        case 'createdAt':
          return formatDate(cellValue as Date)
        default:
          return cellValue as React.ReactNode
      }
    },
    [formatDate]
  )

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

  const topContent = useMemo(
    () => (
      <div className="flex gap-8 mt-8 w-full justify-between">
        <h2>Users List</h2>
        <div className="flex items-end">
          <Input
            isClearable
            className="w-[400px]"
            placeholder="Search by name..."
            startContent={<SearchIcon />}
            value={filterValue}
            onClear={onClear}
            onValueChange={onSearchChange}
          />
        </div>
      </div>
    ),
    [filterValue, onSearchChange, onClear]
  )

  const bottomContent = useMemo(
    () => (
      <div className="py-4 px-2 flex justify-between items-center">
        <span className="w-[30%] text-small text-default-400">
          Page {page} out of {pages}
        </span>
        <Pagination isCompact showControls showShadow color="primary" page={page} total={pages} onChange={setPage} />
        <div className="hidden sm:flex w-[30%] justify-end gap-2">
          <PaginationButtons
            currentIndex={page}
            totalItems={pages}
            onPrevious={() => setPage(page - 1)}
            onNext={() => setPage(page + 1)}
          />
          <Button color="primary" size="sm">
            Export
          </Button>
        </div>
      </div>
    ),
    [page, pages]
  )

  return (
    <>
      <div className="flex flex-col">
        <h2 className="text-2xl font-bold my-4 text-center flex justify-center">Users Management</h2>
        <Table
          isStriped
          isHeaderSticky
          aria-label="Example table with custom cells, pagination"
          bottomContent={bottomContent}
          bottomContentPlacement="outside"
          classNames={{ wrapper: 'min-h-[70vh] max-h-[80vh] overflow-y-auto' }}
          topContent={topContent}
          topContentPlacement="outside"
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
      </div>
      <CommonModal
        isOpen={isEditModalOpen}
        onClose={() => setIsEditModalOpen(false)}
        title="Edit Confirmation"
        content="Do you want to edit this user record?"
        confirmButtonText="Edit"
        onConfirm={() => setIsEditModalOpen(false)}
      />
      <CommonModal
        isOpen={isDeleteModalOpen}
        onClose={() => setIsDeleteModalOpen(false)}
        title="Delete Confirmation"
        content="Do you want to delete this user record?"
        confirmButtonText="Delete"
        onConfirm={() => setIsDeleteModalOpen(false)}
      />
    </>
  )
}
