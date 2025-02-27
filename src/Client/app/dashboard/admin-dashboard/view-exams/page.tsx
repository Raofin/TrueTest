"use client";

import React, { useEffect, useState } from "react";
import axios from "axios";
import { Button, Card, CardBody, CardHeader } from "@heroui/react";


interface Exam {
  title: string;
  description: string;
  durationMinutes: number;
  opensAt: string;
  closesAt: string;
}

export default function ExamList() {
  const [exams, setExams] = useState<Exam[]>([]);
  const [loading, setLoading] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchExams = async () => {
      try {
        const response = await axios.get<Exam[]>(`${process.env.NEXT_PUBLIC_URL}/Exam`, {
          headers: { "Content-Type": "application/json" },
        });
        setExams(response.data);
      } catch (err) {
        setError("Failed to fetch exams");
      } finally {
        setLoading(false);
      }
    };

    fetchExams();
  }, []);

  if (loading) return <p>Loading exams...</p>;
  if (error) return <p className="text-red-500">{error}</p>;

  return (
    <div className="p-4 h-screen overflow-y-auto">
      {exams.map((exam, index) => (
        <Card key={index} className="relative w-full border-small mb-3">
          <CardHeader>
            <div className="flex justify-between items-center">
              <h1 className="text-2xl font-bold w-full">
                {exam.title} <span className="text-green-500 text-sm">{exam.description}</span>
              </h1>
              <Button className="primary">Edit</Button>
            </div>
          </CardHeader>
          <CardBody className="px-3">
            <div className="flex">
              <div className="flex flex-col flex-1">
                <p>Duration: {exam.durationMinutes} minutes</p>
                <p>Opens At: {new Date(exam.opensAt).toLocaleString()}</p>
                <p>Closes At: {new Date(exam.closesAt).toLocaleString()}</p>
              </div>
            </div>
          </CardBody>
        </Card>
      ))}
    </div>
  );
}
