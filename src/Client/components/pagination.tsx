'use client'

import { Select, SelectItem } from '@heroui/react'

interface PaginationProps {
  rowsPerPage: number;
  setRowsPerPage: (val: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({  rowsPerPage,setRowsPerPage }) => {
  return (
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
  )
}
export default Pagination
