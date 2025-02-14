"use client";

import { useDashboard } from "../DashboardContext";
import {Card, CardBody, Chip, Tab, Tabs} from "@heroui/react";
import {
  Navbar, NavbarContent, NavbarItem, Link, Button, Dropdown, DropdownTrigger, DropdownMenu, DropdownItem, Popover, PopoverContent, PopoverTrigger, Avatar, Badge,
} from "@nextui-org/react";
import {Icon} from "@iconify/react";
import React, {useState} from "react";
import Candidate from '../admin_dashboard/candidate-management/page'
import Exam from '../admin_dashboard/exam-management/page'
import Live from '../admin_dashboard/live-monitoring/page'
import Reviewer from '../admin_dashboard/reviewer-management/page'
import AttendExam from '../candidate_dashboard/attend-exam/page'
import ExamSchedule from '../candidate_dashboard/exam-schedule/page'
import ViewResult from '../candidate_dashboard/view-result/page'
import Review from '../reviewer_dashboard/review-list/page'
import NotificationsCard from "./notifications-card";
export default function Component() {
  const {dashboardType} = useDashboard();

  const [selected, setSelected] = useState<string>("home");
  return (
      <div >
        <Tabs className={'w-full flex justify-center'} aria-label="Options" selectedKey={selected} onSelectionChange={(key) => setSelected(key as string)}>
            <Tab title={<span className="opacity-100 ">OPS</span>} className="bg-none shadow-none border-none pointer-events-none font-extrabold text-3xl" isDisabled={true}  />

          {dashboardType === 'admin' ?
              <>
                  <Tab key="home" title="Home" className={'ml-80'}/>
                <Tab key="candidate" title="Candidate">
                  <Card className="w-full ">
                    <CardBody>
                      <Candidate/>
                    </CardBody>
                  </Card>
                </Tab>
                <Tab key="exam" title="Exam">
                  <Card className="w-full">
                    <CardBody>
                      <Exam/>
                    </CardBody>
                  </Card>
                </Tab>
                <Tab key="reviewer" title="Reviewer">
                  <Card className="w-full">
                    <CardBody>
                      <Reviewer/>
                    </CardBody>
                  </Card>
                </Tab>
                <Tab key="live" title="Live">
                  <Card className="w-full">
                    <CardBody>
                      <Live/>
                    </CardBody>
                  </Card>
                </Tab>
              </>
              : dashboardType === 'candidate' ? < >
                      <Tab key="home" title="Home" className={'ml-80'}/>
                    <Tab key="attend-exam" title="Attend Exam">
                      <Card>
                        <CardBody>
                          <AttendExam/>
                        </CardBody>
                      </Card>
                    </Tab>
                    <Tab key="exam-schedule" title="Exam Schedule">
                      <Card>
                        <CardBody>
                          <ExamSchedule/>
                        </CardBody>
                      </Card>
                    </Tab>
                    <Tab key="viewresult" title="View Result">
                      <Card>
                        <CardBody>
                          <ViewResult/>
                        </CardBody>
                      </Card>
                    </Tab>
                  </>
                  : dashboardType === 'reviewer' ?<>
                          <Tab key="home" title="Home" className={'ml-80'}/>
                        <Tab key="reviewer" title="Review">
                          <Card className="w-full ">
                            <CardBody>
                              <Review/>
                            </CardBody>
                          </Card>
                        </Tab>
                  </>
                      : ""
          }

            <Tab className={'ml-16'}>
                <Link href="/settings/1"> <Icon className="text-default-500" icon="solar:settings-linear" width={24} />
                </Link>
            </Tab>
            <Tab>
              <Popover offset={12} placement="bottom-end">
                <PopoverTrigger>
                    <Icon className="text-default-500" icon="solar:bell-linear" width={22} />
                </PopoverTrigger>
                <PopoverContent className="max-w-[90vw] p-0 sm:max-w-[380px]">
                  <NotificationsCard className="w-full shadow-none" />
                </PopoverContent>
              </Popover>
            </Tab>
            <Tab className={'mr-5'}>
              <Dropdown placement="bottom-end">
                <DropdownTrigger>
                  <div className="mt-1 h-8 w-8 outline-none transition-transform">
                    <Badge
                        className="border-transparent"
                        color="success"
                        content=""
                        placement="bottom-right"
                        shape="circle"
                        size="sm">
                      <Avatar size="sm" src="" />
                    </Badge>
                  </div>
                </DropdownTrigger>
                <DropdownMenu aria-label="Profile Actions" variant="flat">
                  <DropdownItem key="profile" className="h-14 gap-2">
                    <p className="font-semibold">Signed in as</p>
                    <p className="font-semibold">johndoe@example.com</p>
                  </DropdownItem>
                  <DropdownItem key="settings"><Link href="/myprofile/1">My Profile</Link></DropdownItem>
                  <DropdownItem key="logout" color="danger">
                    <Link href="/"> Log Out</Link>
                  </DropdownItem>
                </DropdownMenu>
              </Dropdown>
            </Tab>

        </Tabs>
      </div>
  )
}
                //
                //
                // <Link href="/login">
                //   <Button color="primary" variant="shadow">
                //     Login
                //   </Button>
                // </Link>

