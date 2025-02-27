import React from "react";
import { Tabs, Tab, Card, CardBody } from "@heroui/react";
import { Icon } from "@iconify/react";
import Candidate from '../users-management/page'
import Exams from '../view-exams/page'
import Admin from '../admin-management/page'
import InviteCandidates from '../invite-candidates/page'
import CreateExams from '../create-exams/page'
import Home from '../home/page'
export default function App() {
  const [selected, setSelected] = React.useState("dashboard");

  return (
    <div className="flex flex-col w-full h-screen bg-default-50 mt-12 text-left shadow">
       {/* <div className="flex items-center gap-2 text-sm ml-3" >
            <Icon icon="lucide:user" width={32} height={32} />
            <div className="text-left gap-1">
                <h3 className="font-semibold">Name</h3>
                <p className="text-small text-default-500">Customer Support</p>
              </div>
          </div> */}
        <Tabs  
          aria-label="Navigation"
          selectedKey={selected}
          onSelectionChange={(key) => setSelected(key.toString())}
          className="w-56 p-4 bg-content1 flex flex-col text-left h-screen items-start"
          variant="light"
          isVertical >
          <Tab  key="dashboard"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:layout-dashboard" width={20} />
                <span>Dashboard</span>
              </div>
            } >
            <Card>
              <CardBody>
                <h2 className="text-2xl font-bold mb-4 text-center w-full">Overview</h2>
                <Home/>
              </CardBody>
            </Card>
          </Tab>
          <Tab key="viewexams"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:book-open" width={20}/>
                <span>View Exams</span>
              </div>
            }>
            <Card  className="w-[1070px] ">
              <CardBody>
                <Exams/>
              </CardBody>
            </Card>
          </Tab>
          <Tab key="createexams"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:plus-circle" width={20} />
                <span>Create Exams</span>
              </div>
            }>
            <Card  className="w-[1070px] ">
              <CardBody>
                <h2 className="text-xl font-bold mb-4">Create Exams</h2>
                <CreateExams/>
              </CardBody>
            </Card>
          </Tab>
          <Tab key="invitecandidates"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:users" width={20} />
                <span>Invite Candidates</span>
              </div>
            }>
            <Card  className="w-[1070px] ">
              <CardBody>
                <InviteCandidates/>
              </CardBody>
            </Card>
          </Tab>
          <Tab key="moderateexam"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:shield" width={20} />
                <span>Moderate Exams</span>
              </div>
            } >
            <Card  className="w-[1070px] ">
              <CardBody>
                
              </CardBody>
            </Card>
          </Tab>
          <Tab  key="users"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:user" width={20} />
                <span>Users Management</span>
              </div>
            }>
            <Card  className="w-[1070px] ">
              <CardBody>
               <Candidate/>
              </CardBody>
            </Card>
          </Tab>
          <Tab key="admins"
            title={
              <div className="flex items-center gap-2">
                <Icon icon="lucide:settings" width={20} />
                <span>Admin Management</span>
              </div>
            }>
            <Card className="w-[1070px] ">
              <CardBody className="w-full">
              <Admin/>
              </CardBody>
            </Card>
          </Tab>
        </Tabs>
      </div>
  );
}