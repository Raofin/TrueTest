'use client'

import { usePathname } from 'next/navigation'
import { Icon } from '@iconify/react'
import Link from 'next/link'
import { useState } from 'react'
import Logo from '@/app/components/ui/logo/page'
import ThemeSwitch from '@/app/ThemeSwitch'

const menuItems = [
  { key: 'dashboard', icon: 'lucide:layout-dashboard', label: 'Dashboard', path: '/overview' },
  { key: 'viewexams', icon: 'lucide:book-open', label: 'View Exams', path: '/view-exams' },
  { key: 'createexams', icon: 'lucide:plus-circle', label: 'Create Exams', path: '/exams/create' },
  { key: 'invitecandidates', icon: 'lucide:users', label: 'Invite Candidates', path: '/invite-candidates' },
  { key: 'users', icon: 'lucide:user', label: 'Users Management', path: '/manage-users' },
  { key: 'admins', icon: 'lucide:settings', label: 'Admin Management', path: '/add-admins' },
]

const Sidebar = () => {
  const pathname = usePathname()
  const [isCollapsed, setIsCollapsed] = useState(false)

  return (
    <div className={`w-64 flex flex-col justify-between`}>
      <div
        className={`flex flex-col justify-between min-h-screen max-h-fit  ${
          isCollapsed ? 'w-20' : 'w-56'
        } transition-all duration-300 border-r border-white/10`}
      >
        <div>
          <div className="flex flex-col pt-3 pl-2">
            <div className="flex w-full justify-between ">
              {!isCollapsed && <Logo />}
              <button onClick={() => setIsCollapsed(!isCollapsed)} className="p-2 hover:bg-white/10 rounded-lg">
                <Icon icon={isCollapsed ? 'lucide:chevron-right' : 'lucide:chevron-left'} width={20} />
              </button>
            </div>
          </div>
          <ul className="pt-3">
            {menuItems.map((item) => (
              <li
                key={item.key}
                className={`flex items-start gap-3 p-3 rounded-lg hover:bg-white/10 
              ${pathname === item.path ? 'bg-primary/20 text-primary' : ''}`}
              >
                <Icon icon={item.icon} width={20} />
                {!isCollapsed && (
                  <Link href={item.path} className="w-full">
                    {item.label}
                  </Link>
                )}
              </li>
            ))}
          </ul>
        </div>

        <div className="border-t border-white/10 px-2 py-4 mb-5 sticky bottom-0">
          <hr />
          <div className="flex flex-col gap-1 pt-3">
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-full flex items-center justify-center">
                <Icon icon="lucide:user" width={16} />
              </div>
              {!isCollapsed && (
                <Link href={`/profile`}>
                  <div>
                    <p className={`text-sm `}>admin</p>
                    <p className={`text-xs `}>admin@gmail.com</p>
                  </div>
                </Link>
              )}
            </div>
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-full flex items-center justify-center">
                <Icon icon="lucide:settings" width={16} />
              </div>
              {!isCollapsed && (
                <div className="flex">
                  <Link href="/settings">
                    <p className={``}>Settings</p>
                  </Link>
                </div>
              )}
            </div>
            <div className="flex items-center gap-2 ml-2">
              <ThemeSwitch />
              {!isCollapsed && <button>Theme</button>}
            </div>
            <div className="flex items-center gap-2">
              <div className="w-8 h-8 rounded-full flex items-center justify-center">
                <Icon icon="lucide:log-out" width={16} />
              </div>
              {!isCollapsed && (
                <div>
                  <Link href="/login">
                    <p className={`text-sm`}>Log Out</p>
                  </Link>
                </div>
              )}
            </div>
          </div>
        </div>
      </div>
    </div>
  )
}

export default Sidebar
