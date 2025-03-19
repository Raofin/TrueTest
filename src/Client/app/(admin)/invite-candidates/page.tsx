'use client'

import React, { useCallback, useMemo, useRef, useState } from 'react'
import { MdDelete } from 'react-icons/md'
import { FaEdit } from 'react-icons/fa'
import Papa from 'papaparse'
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
  Textarea,
  Tooltip,
  Select,
} from '@heroui/react'
import SearchIcon from '@/app/components/table/search_icon/page'
import { Icon } from '@iconify/react/dist/iconify.js'
import CommonModal from '@/app/components/ui/Modal/edit-delete-modal'
import PaginationButtons from '@/app/components/ui/pagination-button'


const columns = [
  { label: 'Email', key: 'email' },
  { label: 'Action', key: 'action' },
]
interface CsvRow {
  email?: string
  [key: string]: string | undefined
}
// const users = [
//   {
//     key: '1',
//     email: 'alice@example.com',
//   },
//   {
//     key: '2',
//     email: 'bob@example.com',
//   },
//   {
//     key: '3',
//     email: 'charlie@example.com',
//   },
//   {
//     key: '4',
//     email: 'david@example.com',
//   },
//   {
//     key: '5',
//     email: 'eve@example.com',
//   },
//   {
//     key: '6',
//     email: 'eve@example.com',
//   },
//   {
//     key: '7',
//     email: 'eve@example.com',
//   },
//   {
//     key: '8',
//     email: 'eve@example.com',
//   },
//   {
//     key: '9',
//     email: 'eve@example.com',
//   },
//   {
//     key: '10',
//     email: 'eve@example.com',
//   },
// ]
type User = {
  email: string
}
export default function Component() {
  const exams: { key: string; label: string }[] = [
    { key: 'a', label: 'Learnathon 1.0' },
    { key: 'b', label: 'Learnathon 2.0' },
    { key: 'c', label: 'Learnathon 3.0' },
  ]
  const [filterValue, setFilterValue] = useState('')
  const rowsPerPage = 7
  const [page, setPage] = useState(1)
  const [fileContent, setFileContent] = useState('')
  const fileInputRef = useRef<HTMLInputElement | null>(null)
  const [copiedEmail, setCopiedEmail] = React.useState('')
  const hasSearchFilter = Boolean(filterValue)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [userEmail, setUserEmail] = useState<User[]>([])
  const handleCopyEmail = useCallback(
    (email: string) => {
      navigator.clipboard.writeText(email).then(() => {
        setCopiedEmail(email)
        setTimeout(() => setCopiedEmail(''), 2000)
      })
    },
    [setCopiedEmail]
  )

  const filteredItems = useMemo(() => {
    let filteredUsers = [...userEmail]
    if (hasSearchFilter) {
      filteredUsers = filteredUsers.filter((user) => user.email.toLowerCase().includes(filterValue.toLowerCase()))
    }
    return filteredUsers
  }, [filterValue, hasSearchFilter, userEmail])

  const pages =( Math.ceil(filteredItems.length / rowsPerPage) || 1)

  const items = useMemo(() => {
    const start = (page - 1) * rowsPerPage
    const end = start + rowsPerPage
    return filteredItems.slice(start, end)
  }, [page, filteredItems, rowsPerPage])

  const handleFileUpload = (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0]
    if (file) {
      const reader = new FileReader()
      reader.onload = (e) => {
        if (e.target?.result) {
          setFileContent(e.target.result as string)
        }
      }
      reader.readAsText(file)
    }
  }
  const handlecsvtoarray = () => {
    Papa.parse(fileContent, {
      header: true,
      skipEmptyLines: true,
      complete: (result) => {
        const emailList = (result.data as CsvRow[])
          .map((row) => row.email || Object.values(row)[0])
          .filter((email): email is string => typeof email === 'string')

        const userList: User[] = emailList.map((email) => ({ email }))
        setUserEmail(userList)
      },
      error: (error: unknown) => console.error('Parsing Error: ', error),
    })
  }

  const renderCell = useCallback(
    (user: User, columnKey: React.Key) => {
      const cellValue = user[columnKey as keyof User]
      switch (columnKey) {
        case 'action':
          return (
            <div className="flex justify-center gap-4">
              <button onClick={() => setIsEditModalOpen(true)}>
                <FaEdit className="text-xl" />
              </button>
              <button onClick={() => setIsDeleteModalOpen(true)}>
                <MdDelete className="text-xl" />
              </button>
            </div>
          )
        default:
          return (
            <div className="flex items-center gap-2">
              <span>{cellValue}</span>
              <Tooltip content={copiedEmail === cellValue ? 'Copied!' : 'Copy email'}>
                <Button isIconOnly variant="light" size="sm" onPress={() => handleCopyEmail(cellValue)}>
                  <Icon
                    icon={copiedEmail === cellValue ? 'lucide:check' : 'lucide:copy'}
                    className={copiedEmail === cellValue ? 'text-success' : ''}
                    width={18}
                  />
                </Button>
              </Tooltip>
            </div>
          )
      }
    },
    [copiedEmail, handleCopyEmail]
  )

  const onSearchChange = useCallback((value?: string) => {
    if (value) {
      setFilterValue(value)
      setPage(1)
    } else {
      setFilterValue('')
    }
  }, [])

  const topContent = useMemo(
    () => (
      <div className="mt-4 flex gap-8 w-full justify-between">
        <h2>Candidates List</h2>
        <div className="flex items-end">
          <Input
            isClearable
            className="w-[400px] "
            placeholder="Search by name..."
            startContent={<SearchIcon />}
            value={filterValue}
            onValueChange={onSearchChange}
          />
        </div>
      </div>
    ),
    [filterValue, onSearchChange]
  )

  const bottomContent = useMemo(
    () => (
      <div className="py-2 px-2 flex justify-between items-end">
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
          <Button size="sm">Send Invitation</Button>
        </div>
      </div>
    ),
    [page, pages]
  )

  return (
    <div className={`mx-3 flex flex-col `}>
      <div>
        <Select className="max-w-xs my-5 ml-4" label="Select an exam">
          {exams.map((exam) => (
            <SelectItem key={exam.key}>{exam.label}</SelectItem>
          ))}
        </Select>
      </div>
      <div className="flex gap-4">
        <Textarea type="file" value={fileContent} readOnly />
        <div className="flex flex-col gap-4">
          <input
            type="file"
            accept=".csv,.txt"
            ref={fileInputRef}
            onChange={handleFileUpload}
            style={{ display: 'none' }}
          />
          <Button onPress={() => fileInputRef.current?.click()}>Upload CSV</Button>
          <Button onPress={handlecsvtoarray}>Add to list</Button>
        </div>
      </div>
      <Table
        isStriped
        isHeaderSticky
        aria-label="Example table with custom cells, pagination"
        bottomContent={bottomContent}
        bottomContentPlacement="outside"
        className=""
        classNames={{
          wrapper: 'min-h-[50vh] max-h-[80vh] overflow-y-auto',
          table: 'w-full',
        }}
        topContent={topContent}
        topContentPlacement="outside"
        selectionMode="multiple"
      >
        <TableHeader>
          {columns.map((column) => (
            <TableColumn key={column.key} align="center" className="font-semibold">
              {column.label}
            </TableColumn>
          ))}
        </TableHeader>
        <TableBody emptyContent="No candidate found" className={items.length === 0 ? 'min-h-[50vh]' : 'min-h-[auto]'}>
          {items.map((item) => (
            <TableRow key={item.email} className="max-h-4">
              {columns.map((column) => (
                <TableCell key={column.key} align="center" className="max-h-4">
                  {renderCell(item, column.key)}
                </TableCell>
              ))}
            </TableRow>
          ))}
        </TableBody>
      </Table>

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
    </div>
  )
}
