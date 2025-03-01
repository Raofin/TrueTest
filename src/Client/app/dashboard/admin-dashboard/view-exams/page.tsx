"use client";

import React from "react";
import { Button, Card, CardBody, CardHeader } from "@heroui/react";

interface Exam {
  title: string;
  description: string;
  durationMinutes: number;
  opensAt: string;
  closesAt: string;
  status: "Published" | "Draft" | "Review Results"; 
  invitedCandidates: number | "N/A"; 
  acceptedCandidates: number | "N/A"; 
  problemSolving: number; 
  written: number; 
  mcq: number; 
  score: number; 
}

const Exams: Exam[] = [
  {
    title: "Star Coder 2026",
    description: "Published",
    durationMinutes: 60,
    opensAt: "2026-11-21T21:00:00.000Z",
    closesAt: "2026-11-21T22:20:00.000Z",
    status: "Published",
    invitedCandidates: 3068,
    acceptedCandidates: 2613,
    problemSolving: 10,
    written: 10,
    mcq: 30,
    score: 100,
  },
  {
    title: "Learnathon 4.0",
    description: "Draft",
    durationMinutes: 90,
    opensAt: "2026-12-10T21:00:00.000Z",
    closesAt: "2026-12-10T23:00:00.000Z",
    status: "Draft",
    invitedCandidates: "N/A",
    acceptedCandidates: "N/A",
    problemSolving: 10,
    written: 10,
    mcq: 30,
    score: 100,
  },
  {
    title: "Star Coder 2005",
    description: "Review Results",
    durationMinutes: 60,
    opensAt: "2025-11-21T21:00:00.000Z",
    closesAt: "2025-11-21T22:00:00.000Z",
    status: "Review Results",
    invitedCandidates: 3068,
    acceptedCandidates: 2613,
    problemSolving: 10,
    written: 10,
    mcq: 30,
    score: 100,
  },
];

export default function ExamList() {
  return (
    <div className="p-4 h-screen overflow-y-auto">
      {Exams.map((exam, index) => (
        <Card key={index} className="relative w-full border-small mb-3">
          <CardHeader>
            <div className="flex justify-between items-center gap-44">
              <h1 className="text-2xl font-bold w-full">
                {exam.title} <span className={`text-sm ${exam.status === "Published" ? "text-green-500" : exam.status === "Draft" ? "text-gray-500" : "text-blue-500"}`}>{exam.description}</span>
              </h1>
              {exam.status === "Published" ? (
                <Button className="primary ml-96">Edit</Button>
              ) : exam.status === "Draft" ? (
                <div className="flex gap-2">
                  <Button className="primary ml-96">Edit</Button>
                  <Button className="bg-blue-500 text-white">Publish</Button>
                </div>
              ) : (
                <Button className="bg-purple-500 text-white ml-96">Review</Button>
              )}
            </div>
          </CardHeader>
          <CardBody className="px-3">
            <div className="flex">
              <div className="flex flex-col flex-1">
                <p>Date: {new Date(exam.opensAt).toLocaleDateString("en-US", { weekday: 'long', year: 'numeric', month: 'long', day: 'numeric' })}</p>
                <p>Duration: {exam.durationMinutes / 60} hr</p>
                <p>Starts at: {new Date(exam.opensAt).toLocaleTimeString("en-US", { hour: '2-digit', minute: '2-digit' })}</p>
                <p>Closes at: {new Date(exam.closesAt).toLocaleTimeString("en-US", { hour: '2-digit', minute: '2-digit' })}</p>
              </div>
              <div className="flex flex-col flex-1">
                <p>Problem Solving: {exam.problemSolving}</p>
                <p>Written: {exam.written}</p>
                <p>MCQ: {exam.mcq}</p>
                <p>Score: {exam.score}</p>
              </div>
              <div className="flex flex-col flex-1">
                <p>Invited Candidates: {exam.invitedCandidates}</p>
                <p>Accepted: {exam.acceptedCandidates}</p>
              </div>
            </div>
          </CardBody>
        </Card>
      ))}
    </div>
  );
}