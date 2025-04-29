'use client'

import { Select, SelectItem } from '@heroui/react'

interface RoleFilterProps {
  roleFilter: string;
  onRoleChange: (role: string) => void;
}

const RoleFilter: React.FC<RoleFilterProps> = ({  roleFilter,onRoleChange }) => {
  return (

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

  )
}
export default RoleFilter
