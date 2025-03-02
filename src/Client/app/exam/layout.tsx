'use client'

import React from 'react';
import NavBar from './navbar/page';

interface LayoutProps {
  children: React.ReactNode;
}

export default function RootLayout({ children }: LayoutProps) {
  const [examStarted, setExamStarted] = React.useState(false);
  const [currentPage, setCurrentPage] = React.useState(1);
  const [timeLeft, setTimeLeft] = React.useState(3600);
  const examData = {
    title: "Star Coder 2025",
    totalQuestions: 3,
    duration: "1:00:00",
    problemSolving: 10,
    written: 10,
    mcq: 30,
    totalScore: 100
  };

  const formatTime = (seconds: number) => {
    const hours = Math.floor(seconds / 3600);
    const minutes = Math.floor((seconds % 3600) / 60);
    const secs = seconds % 60;
    return `${hours}:${minutes.toString().padStart(2, '0')}:${secs.toString().padStart(2, '0')}`;
  };

  return (
    <>
        {examStarted && (
          <NavBar
            title={examData.title}
            totalQuestions={examData.totalQuestions}
            currentPage={currentPage}
            timeLeft={timeLeft}
            formatTime={formatTime}
          />
        )}
        {children}
    </>
  );
}