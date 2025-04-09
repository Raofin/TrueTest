'use client'

import React, { useCallback, useEffect, useMemo, useRef, useState } from 'react'
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
import SearchIcon from '@/components/ui/search-icon'
import { Icon } from '@iconify/react/dist/iconify.js'
import CommonModal from '@/components/ui/Modal/edit-delete-modal'
import PaginationButtons from '@/components/ui/pagination-button'
import api from '@/utils/api'
import toast from 'react-hot-toast'

const columns = [
  { label: 'Email', key: 'email' },
  { label: 'Action', key: 'action' },
]
interface CsvRow {
  email?: string
  [key: string]: string | undefined
}
type Exams = {
  examId: string
  title: string
}
type User = {
  email: string
}

export default function Component() {
  const [filterValue, setFilterValue] = useState('')
  const rowsPerPage = 10
  const [page, setPage] = useState(1)
  const [fileContent, setFileContent] = useState('')
  const fileInputRef = useRef<HTMLInputElement | null>(null)
  const [exams, setExams] = useState<Exams[]>([])
  const [copiedEmail, setCopiedEmail] = React.useState('')
  const hasSearchFilter = Boolean(filterValue)
  const [isEditModalOpen, setIsEditModalOpen] = useState(false)
  const [isDeleteModalOpen, setIsDeleteModalOpen] = useState(false)
  const [userEmail, setUserEmail] = useState<User[]>([])
  const [examId,setExamId]=useState("")

  useEffect(() => {
    const SelectExams = async () => {
      try {
        const response = await api.get(`/Exam`)
        if (response.status === 200) {
          setExams(response.data.accounts)
        }
      } catch {
        toast.error('An error has occurered')
      }
    }
    SelectExams()
  }, [])
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

  const pages = Math.ceil(filteredItems.length / rowsPerPage) || 1

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
  const handleSendToAPI = async (examId: string, emails: User[]) => {
    try {
      const response = await api.post('/Exam/Invite/Candidates',{examId,emails});

      if (response.status!==200) {
        throw new Error('Failed to invite candidates');
      }
    } catch (error) {
      console.error('Error sending to API:', error);
    }
  };  
  const handlecsvtoarray = () => {
    if (!fileContent.includes(',') && !fileContent.includes('\n') && !fileContent.includes('\t')) {
      setUserEmail([{ email: fileContent.trim() }]);
      return;
    }
    Papa.parse(fileContent, {
      header: true,
      skipEmptyLines: true,
      complete: (result) => {
        if (result.data.length > 0 && typeof result.data[0] === 'object') {
          const emailList = (result.data as CsvRow[])
            .map((row) => row.email ?? Object.values(row)[0])
            .filter((email): email is string => typeof email === 'string');
          setUserEmail(emailList.map((email) => ({ email })));
        } else {
          parsePlainTextEmails();

        }
      },
      error: () => {
        parsePlainTextEmails();

      },
    });

  };
  
  const parsePlainTextEmails = () => {
    const emails = fileContent
      .split(/[\n,]+/) 
      .map(email => email.trim())
      .filter(email => email.length > 0 && email.includes('@')); 
  
    setUserEmail(emails.map(email => ({ email })));
  };

  const renderCell = useCallback(
    (user: User, columnKey: React.Key) => {
      const cellValue = user[columnKey as keyof User]
      if (columnKey === 'action') {
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
      } else {
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

  return (
    <div className="flex  flex-col justify-between">
      <h2 className="text-2xl font-bold my-5 text-center flex justify-center"> Invite Candidates</h2>
      <div className={`min-h-screen flex flex-col rounded-xl pt-5 justify-between mx-12 bg-white dark:bg-[#18181b]`}>
        <div>
          <div className="w-full flex items-center justify-center">
            <p>Exam</p>
            <Select
              label=""
              className="max-w-md  ml-4 bg-[#eeeef0] dark:bg-[#27272a] rounded-2xl"
              placeholder="Select an exam"   
            >
              {exams?.map((exam) => (
                <SelectItem onChange={()=>{setExamId(exam.examId)}} key={exam.examId}>{exam.title}</SelectItem>
              ))}
            </Select>
          </div>
          <h1 className="ml-6 my-2">Candidate Email Import</h1>
          <div className="flex gap-2 px-5">
            <Textarea
              value={fileContent}
              onChange={(e) => setFileContent(e.target.value)}
              placeholder="Insert candidate emails (separated by line breaks or commas) or upload a file."
              rows={5}
              onPaste={(e) => {
                const items = e.clipboardData.items
                for (let i = 0; i < items.length; i++) {
                  if (items[i].kind === 'file') {
                    const file = items[i].getAsFile()
                    if (file) {
                      const reader = new FileReader()
                      reader.onload = (event) => {
                        if (event.target?.result) {
                          setFileContent(event.target.result as string)
                        }
                      }
                      reader.readAsText(file)
                    }
                  }
                }
              }}
            />
            <div className="flex flex-col gap-2">
              <input
                type="file"
                accept=".csv,.txt,.xlsx"
                ref={fileInputRef}
                onChange={handleFileUpload}
                style={{ display: 'none' }}
              />
              <Button onPress={() => fileInputRef.current?.click()}>Upload CSV</Button>
              <Button color="primary" onPress={handlecsvtoarray} isDisabled={!fileContent}>
                Add to list
              </Button>
            </div>
          </div>
        </div>
        <div>
          <div className="my-4 flex w-full justify-between">
            <h2 className="ml-5">Candidates List</h2>
            <div className="flex items-end">
              <Input
                isClearable
                className="w-[400px] mr-3"
                placeholder="Search"
                startContent={<SearchIcon />}
                value={filterValue}
                onValueChange={onSearchChange}
              />
            </div>
          </div>
          <Table
            suppressHydrationWarning
            aria-label="Example table with custom cells, pagination"
            bottomContentPlacement="inside"
            className=""
            topContentPlacement="inside"
            selectionMode="multiple"
          >
            <TableHeader>
              {columns.map((column) => (
                <TableColumn key={column.key} align="center" className="font-semibold">
                  {column.label}
                </TableColumn>
              ))}
            </TableHeader>
            <TableBody emptyContent="No candidate found" className="">
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
        </div>
        <div className="p-2 m-2 flex justify-between items-end">
          <Pagination isCompact showControls showShadow color="primary" page={page} total={pages} onChange={setPage} />
          <div className="flex items-center  gap-2">
            <span className="text-small ">
              Page {page} out of {pages}
            </span>
            <PaginationButtons
              currentIndex={page}
              totalItems={pages}
              onPrevious={() => setPage(page - 1)}
              onNext={() => setPage(page + 1)}
            />
            <Button size="sm" color="primary" onPress={()=>handleSendToAPI(examId,userEmail)}>
              Send Invitation
            </Button>
          </div>
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
      </div>
    </div>
  )
}
