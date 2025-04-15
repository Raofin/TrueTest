'use client'

import React from 'react'
import { Avatar, Badge,cn } from '@heroui/react'

export type NotificationType = 'default' | 'request'

export type NotificationItem = {
  id: string
  isRead?: boolean
  avatar: string
  description: string
  name: string
  time: string
}

export type NotificationItemProps = React.HTMLAttributes<HTMLDivElement> & NotificationItem

const NotificationItem = React.forwardRef<HTMLDivElement, NotificationItemProps>(
  ({ children, avatar, name, description, time, isRead, className, ...props }, ref) => {
    return (
      <div
        ref={ref}
        className={cn(
          'flex gap-3 border-b border-divider px-6 py-4',
          {
            'bg-primary-50/50': !isRead,
          },
          className
        )}
        {...props}
      >
        <div className="relative flex-none">
          <Badge color="primary" content="" isInvisible={isRead} placement="bottom-right" shape="circle">
            <Avatar src={avatar} />
          </Badge>
        </div>
        <div className="flex flex-col gap-1">
          <p className="text-small text-foreground">
            <strong className="font-medium">{name}</strong> {description ?? children}
          </p>
          <time className="text-tiny text-default-400">{time}</time>
        </div>
      </div>
    )
  }
)

NotificationItem.displayName = 'NotificationItem'

export default NotificationItem
