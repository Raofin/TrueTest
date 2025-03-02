'use client';

import React, { useState, useEffect } from "react";
import { Button } from "@heroui/react";
import { Icon } from "@iconify/react/dist/iconify.js";

interface NavBarProps {
  title: string;
  totalQuestions: number;
  currentPage: number;
  timeLeft: number;
  formatTime: (seconds: number) => string;
}

const NavBar = ({ title, totalQuestions, currentPage, timeLeft, formatTime }: NavBarProps) => {
  return (
    <div className="fixed top-0 left-0 right-0 border-b border-white/10 p-4 z-50 shadow bg-white dark:bg-black dark:text-white"> 
      <div className="max-w-7xl mx-auto flex items-center justify-between">
        <div className="flex items-center gap-2 text-black dark:text-white">
          <span className="font-bold">{title}</span>
        </div>
        <div className="flex items-center gap-4 text-black dark:text-white">
          <div>
          Questions Left: {totalQuestions - currentPage}/{totalQuestions}
          </div>
          <div className={` font-mono ${timeLeft < 300 ? 'text-danger' : ''}`}>
            Time Left: {formatTime(timeLeft)}
          </div>
          <Button color="primary" size="sm">
            Submit Exam
          </Button>
        </div>
      </div>
    </div>
  );
};

export default NavBar;