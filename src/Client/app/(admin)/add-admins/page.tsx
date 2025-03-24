'use client'

import React, { useCallback, useMemo, useState } from 'react'
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
import SearchIcon from 'app/components/table/search_icon/page'

const columns = [
  { label: 'Username', key: 'username' },
  { label: 'Email', key: 'email' },
  { label: 'Role', key: 'role' },
  { label: 'Created At', key: 'create' },
]

const users = [
  { key: '1', username: 'Eve', email: 'eve@example.com', role: 'User', create: '30 May 2025, 6:50 PM' },
  { key: '2', username: 'Alice', email: 'alice@example.com', role: 'Admin', create: '15 Jan 2025, 10:15 AM' },
  { key: '3', username: 'Bob', email: 'bob@example.com', role: 'Moderator', create: '22 Mar 2025, 4:45 PM' },
  { key: '4', username: 'Charlie', email: 'charlie@example.com', role: 'Candidate', create: '10 Feb 2025, 9:30 AM' },
  { key: '5', username: 'David', email: 'david@example.com', role: 'Editor', create: '05 Apr 2025, 1:20 PM' },
  { key: '6', username: 'Eve', email: 'eve@example.com', role: 'User', create: '30 May 2025, 6:50 PM' },
]

type User = (typeof users)[0]

export default function Component() {
  const [filterValue, setFilterValue] = useState('')
  const rowsPerPage = 10
  const [page, setPage] = useState(1)
  const hasSearchFilter = Boolean(filterValue)
  const [allUsers, setAllUsers] = useState(users)
  const [selectedKeys, setSelectedKeys] = useState<Selection>(new Set())

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
    return cellValue
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

  const makeAdmin = useCallback(() => {
    setAllUsers((prevUsers) =>
      prevUsers.map((user) => (Array.from(selectedKeys).includes(user.key) ? { ...user, role: 'Admin' } : user))
    )
    setSelectedKeys(new Set())
  }, [selectedKeys])

  const bottomContent = useMemo(
    () => (
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
          <Button color="primary" size="sm" onPress={makeAdmin}>
            Make Admin
          </Button>
        </div>
      </div>
    ),
    [page, pages, onPreviousPage, onNextPage, makeAdmin]
  )

  return (
    <div className='h-screen flex flex-col justify-between'>
      <h2 className="text-2xl font-bold text-center my-5">Add Admins</h2>
      <div className="mx-12 flex h-screen flex-col justify-between  rounded-xl bg-white dark:bg-[#18181b]">
      
        <div className="flex gap-3 w-full p-3 mt-5">
          <Input isClearable className="bg-[#eeeef0] dark:bg-[#71717a] rounded-2xl" placeholder="Email Address" onClear={onClear} />
          <Button color="primary">Send Invitation</Button>
        </div>
     
     <div>
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
      <Table

        aria-label="Example table with custom cells, pagination, and sorting"
        bottomContent={bottomContent}
        bottomContentPlacement="inside"
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
            <TableRow key={item.key}>
              {columns.map((column) => (
                <TableCell key={column.key}>{renderCell(item, column.key)}</TableCell>
              ))}
            </TableRow>
          ))}
        </TableBody>
      </Table>
     </div>
    </div></div>
  )
}
