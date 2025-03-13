import { Card, CardHeader, CardBody } from "@heroui/react";
import axios from "axios";
import { useEffect, useState } from "react";
import { FaBookOpen, FaCheckCircle, FaThList, FaUser, FaUsers } from "react-icons/fa";
import { AiFillPieChart } from "react-icons/ai";

export default function Component() {
  const [quesLen, setQuesLen] = useState(0);
  const [submitLen, setSubmissionLen] = useState(0);

  // useEffect(() => {
  //   const fetchQuestionCount = async () => {
  //     try {
  //       const response = await axios.get(`${process.env.NEXT_PUBLIC_URL}/Question`);
  //       if (response.status === 200) {
  //         if (Array.isArray(response.data)) {
  //           setQuesLen(response.data.length);
  //         } else if (response.data && response.data.count !== undefined) {
  //           setQuesLen(response.data.count);
  //         } else {
  //           console.warn("Question count not found in response:", response.data);
  //           setQuesLen(0);
  //         }
  //       } else {
  //         console.error("Error fetching questions:", response.status);
  //         setQuesLen(0);
  //       }
  //     } catch (error) {
  //       console.error("Error fetching questions:", error);
  //       setQuesLen(0);
  //     }
  //   };
  //   fetchQuestionCount();
  // }, []);

  // useEffect(() => {
  //   const fetchSubmissionCount = async () => {
  //     try {
  //       const response = await axios.get(`${process.env.NEXT_PUBLIC_URL}/WrittenSubmission/All`);
  //       if (response.status === 200) {
  //         if (Array.isArray(response.data)) {
  //           setSubmissionLen(response.data.length);
  //         } else if (response.data && response.data.count !== undefined) {
  //           setSubmissionLen(response.data.count);
  //         } else {
  //           console.warn("Submission count not found in response:", response.data);
  //           setSubmissionLen(0);
  //         }
  //       } else {
  //         console.error("Error fetching submissions:", response.status);
  //         setSubmissionLen(0);
  //       }
  //     } catch (error) {
  //       console.error("Error fetching submissions:", error);
  //       setSubmissionLen(0);
  //     }
  //   };
  //   fetchSubmissionCount();
  // }, []);

  const stats = [
    { icon: <FaUser size={70} />, value: 1424, title: "Total Users", subtitle: "in this platform" },
    { icon: <FaUsers size={70} />, value: 245, title: "New Users", subtitle: "registered this month" },
    { icon: <FaBookOpen size={70} />, value: 425, title: "Total Exams", subtitle: "created in this platform" },
    { icon: <FaThList size={70} />, value: quesLen, title: "Total Questions", subtitle: "created in this platform" },
    { icon: <FaCheckCircle size={70} />, value: submitLen, title: "Total Submissions", subtitle: "attempted by candidates" },
    { icon: <AiFillPieChart size={70} />, value: 75, title: "Average Score", subtitle: "across all candidates" },
  ];
  const Mode=localStorage.getItem("theme")
  return (
    <div className={`flex flex-wrap gap-6 w-full items-center justify-center h-screen ${Mode==="dark"?"bg-black":"bg-white"}`}>
      {stats.map((stat, index) => (
        <Card key={index} className={`py-4 w-[300px] text-center ${Mode==="dark"?"bg-[#18181b]":"bg-white"}`}>
          <CardHeader className="pb-0 pt-2 px-4 flex flex-col items-center">
            {stat.icon}
            <h1 className="font-bold text-5xl">{stat.value}</h1>
          </CardHeader>
          <CardBody className="py-2 mt-4 text-center">
            <h2 className="text-lg font-semibold">{stat.title}</h2>
            <p className="text-sm text-gray-400">{stat.subtitle}</p>
          </CardBody>
        </Card>
      ))}
    </div>
  );
}
