import React from "react";
import { Tabs, Tab, Card, CardBody } from "@heroui/react";
import { Icon } from "@iconify/react";
import Candidate from '../users-management/page';
import Exams from '../view-exams/page';
import Admin from '../admin-management/page';
import InviteCandidates from '../invite-candidates/page';
import CreateExams from '../create-exams/page';
import Home from '../home/page';
import ModerateExam from '../moderate-exam/page'
export default function App() {
  const [selected, setSelected] = React.useState("dashboard");
  const [isCollapsed, setIsCollapsed] = React.useState(false);

  return (
    <div className="flex h-screen">
    <div className={`flex flex-col ${isCollapsed ? "w-20" : "w-64"} h-screen transition-all duration-300  border-r border-white/10`}>
        <div className="flex items-center justify-between p-4 border-b border-white/10">
          {!isCollapsed && (
            <div className="flex items-center gap-2">
              <span className="font-bold text-xl">OPS</span>
            </div>
          )}
          <button 
            onClick={() => setIsCollapsed(!isCollapsed)}
            className="p-2 hover:bg-white/10 rounded-lg"
          >
            <Icon icon={isCollapsed ? "lucide:chevron-right" : "lucide:chevron-left"} width={20} />
          </button>
        </div>

    
        <Tabs
          aria-label="Navigation"
          selectedKey={selected}
          onSelectionChange={(key) => setSelected(key.toString())}
          className="flex-1 p-2"
          variant="light"
          isVertical
          classNames={{
            tab: "flex items-center gap-3 h-12 px-4 rounded-lg hover:bg-white/10 data-[selected=true]:bg-primary/20 data-[selected=true]:text-primary",
            tabList: "flex flex-col gap-1",
            cursor: "bg-transparent",
          }}
        >
          <Tab
            key="dashboard"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:layout-dashboard" width={20} />
                {!isCollapsed && <span>Dashboard</span>}
              </div>
            }
          >
          </Tab>
          <Tab
            key="viewexams"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:book-open" width={20} />
                {!isCollapsed && <span>View Exams</span>}
              </div>
            }>
          </Tab>
          <Tab
            key="createexams"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:plus-circle" width={20} />
                {!isCollapsed && <span>Create Exams</span>}
              </div>
            }>
          </Tab>
          <Tab
            key="invitecandidates"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:users" width={20} />
                {!isCollapsed && <span>Invite Candidates</span>}
              </div>
            }>
          </Tab>

          <Tab
            key="moderateexam"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:shield" width={20} />
                {!isCollapsed && <span>Moderate Exams</span>}
              </div>
            }
          >
          </Tab>

          <Tab
            key="users"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:user" width={20} />
                {!isCollapsed && <span>Users Management</span>}
              </div>
            }
          >
          
          </Tab>

          <Tab
            key="admins"
            title={
              <div className="flex items-center gap-3">
                <Icon icon="lucide:settings" width={20} />
                {!isCollapsed && <span>Admin Management</span>}
              </div>
            }>
          </Tab>
        </Tabs>
        <div className="border-t border-white/10 p-2">
          <div className="flex flex-col gap-2">
           <div className="flex items-center gap-2">
           <div className="w-8 h-8 rounded-full bg-primary/20 flex items-center justify-center">
              <Icon icon="lucide:user" className="text-primary" width={16} />
            </div>
            {!isCollapsed && (
              <div>
                <p className="text-sm font-medium">Administrator</p>
                <p className="text-xs">admin@truetest.com</p>
              </div>
            )}
           </div>
              <div className="flex items-center gap-2">
             <div className="w-8 h-8 rounded-full bg-primary/20 flex items-center justify-center">
              <Icon icon="lucide:settings" className="text-primary" width={16} />
            </div>
             {!isCollapsed && (
              <div>
                <p className="text-sm font-medium">settings</p>
              </div>
            )}</div>
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-full bg-primary/20 flex items-center justify-center">
              <Icon icon="lucide:sun-moon" className="text-primary" width={16} />
            </div>
             {!isCollapsed && (
              <div>
                <p className="text-sm font-medium">Theme</p>
              </div>
            )}</div>
          <div className="flex items-center gap-2">
            <div className="w-8 h-8 rounded-full bg-primary/20 flex items-center justify-center">
              <Icon icon="lucide:log-out" className="text-primary" width={16} />
            </div>
             {!isCollapsed && (
              <div>
                <p className="text-sm font-medium">log out</p>
              </div>
            )}</div>
          </div>
        </div>
        </div>

      <div className="flex-1 overflow-auto">
        <Card className="m-4 border-none">
          <CardBody>
            {selected === "dashboard" && <Home />}
            {selected === "viewexams" && <Exams />}
            {selected === "createexams" && <CreateExams />}
            {selected === "invitecandidates" && <InviteCandidates />}
            {selected === "moderateexam" && <ModerateExam />}
            {selected === "users" && <Candidate />}
            {selected === "admins" && <Admin />}
          </CardBody>
        </Card>
      </div>
    </div>
  );
}