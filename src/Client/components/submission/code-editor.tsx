"use client";

import React, { useEffect } from "react";
import { Button, Card, Select, SelectItem } from "@heroui/react";
import Editor from "@monaco-editor/react";
import api from "@/lib/api";

export interface TestCase {
    testCaseId?: string;
    input: string;
    output: string;
    receivedOutput?: string;
    status?: "success" | "error" | "pending";
}

export interface ProblemQuestion {
    questionId: string;
    examId: string;
    questionType: string;
    statementMarkdown: string;
    points: number;
    difficultyType: string;
    testCases: TestCase[];
}

interface PageProps {
    question: ProblemQuestion;
    setAnswers: React.Dispatch<
        React.SetStateAction<{ [key: string]: string | string[] }>
    >;
    questionId: string;
}

interface CodeState {
    [key: string]: string;
}

const languages = [
    {
        label: "C++",
        value: "cpp",
        defaultCode:
            "#include <bits/stdc++.h>\nusing namespace std;\n\nint main() {\n    // Your code here\n    return 0;\n}",
    },
    { label: "Python", value: "python", defaultCode: "# Your code here" },
    {
        label: "Java",
        value: "java",
        defaultCode:
            "public class Main {\n    public static void main(String[] args) {\n        // Your code here\n    }\n}",
    },
    {
        label: "JavaScript",
        value: "javascript",
        defaultCode: "function solve(input) {\n    // Your code here\n}",
    },
    {
        label: "C",
        value: "c",
        defaultCode:
            "#include <stdio.h>\n\nint main() {\n    // Your code here\n    return 0;\n}",
    },
];

export default function CodeEditor({
    question,
    setAnswers,
    questionId,
}: PageProps) {
    const [selectedLanguage, setSelectedLanguage] = React.useState("cpp");
    const [codeStates, setCodeStates] = React.useState<CodeState>({});
    const [selectedTestCase, setSelectedTestCase] = React.useState<number>(0);
    const [formattedTestCases, setFormattedTestCases] = React.useState<
        TestCase[]
    >([]);

    const displayedTestCases = [formattedTestCases[selectedTestCase]];

    useEffect(() => {
        const initialStates: CodeState = {};
        languages.forEach((lang) => {
            initialStates[lang.value] = lang.defaultCode;
        });
        setCodeStates(initialStates);

        const initializedTestCases = question.testCases.map((tc) => ({
            ...tc,
            receivedOutput: "",
            status: "pending" as const,
        }));
        setFormattedTestCases(initializedTestCases);
    }, [question.testCases]);

    const handleCodeChange = (value: string | undefined) => {
        if (value !== undefined) {
            setCodeStates((prev) => ({
                ...prev,
                [selectedLanguage]: value,
            }));
            setAnswers((prev) => ({
                ...prev,
                [questionId]: value,
            }));
        }
    };

    const handleRun = async () => {
        const payload = {
            questionId: question.questionId,
            Code: codeStates[selectedLanguage],
            progLanguageType:
                selectedLanguage === "cpp"
                    ? "CPlusPlus"
                    : selectedLanguage === "python"
                    ? "Python"
                    : selectedLanguage === "java"
                    ? "Java"
                    : selectedLanguage === "javascript"
                    ? "JavaScript"
                    : selectedLanguage === "c"
                    ? "C"
                    : "Python",
        };
        try {
            const response = await api.post("/Candidate/TestCode", payload);
            if (response.status === 200) {
                const data = await response.data;

                const updated = formattedTestCases.map((testCase) => {
                    const result = data.find(
                        // eslint-disable-next-line @typescript-eslint/no-explicit-any
                        (r: any) => r.testCaseId === testCase.testCaseId
                    );
                    return {
                        ...testCase,
                        receivedOutput: result?.receivedOutput || "",
                        status: result
                            ? result.isAccepted
                                ? "success"
                                : "error"
                            : "pending",
                    } as TestCase;
                });

                setFormattedTestCases(updated);
            } else {
                console.error("Failed to execute code");
            }
        } catch (error) {
            console.error("Error during code execution:", error);
        }
    };

    return (
        <div className="grid grid-cols-2 gap-4">
            <Card className="border-none rounded-lg p-4 shadow-none bg-white dark:bg-[#18181b]">
                <h2 className="text-xl font-bold mb-3">Problem Statement</h2>
                <div className="space-y-4">
                    <div>
                        <p
                            dangerouslySetInnerHTML={{
                                __html: question.statementMarkdown,
                            }}
                        />
                    </div>
                    <div>
                        <h3 className="font-semibold">Test Cases:</h3>
                        {question.testCases.map((testCase, index) => (
                            <div key={index} className="mt-2 p-3 rounded-lg">
                                <div>
                                    <span className="font-semibold">
                                        Input:
                                    </span>{" "}
                                    {testCase.input}
                                </div>
                                <div>
                                    <span className="font-semibold">
                                        Expected Output:
                                    </span>{" "}
                                    {testCase.output}
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </Card>
            <div>
                <Card className="px-3 rounded-lg h-[500px] mb-3">
                    <div className="flex justify-between pt-2">
                        <p className="font-semibold">Code Editor</p>
                        <div className="flex gap-2">
                            <Button onPress={handleRun}>Run</Button>
                            <Select
                                aria-label="Select Language"
                                selectedKeys={[selectedLanguage]}
                                // eslint-disable-next-line @typescript-eslint/no-explicit-any
                                onChange={(e: any) =>
                                    setSelectedLanguage(e.target.value)
                                }
                                className="w-40"
                            >
                                {languages.map((lang) => (
                                    <SelectItem key={lang.value}>
                                        {lang.label}
                                    </SelectItem>
                                ))}
                            </Select>
                        </div>
                    </div>
                    <div className="m-3 rounded-lg overflow-hidden">
                        <Editor
                            height="420px"
                            defaultLanguage={selectedLanguage}
                            language={selectedLanguage}
                            value={codeStates[selectedLanguage]}
                            onChange={handleCodeChange}
                            theme="vs-dark"
                            options={{
                                minimap: { enabled: false },
                                fontSize: 14,
                                lineNumbers: "on",
                                scrollBeyondLastLine: false,
                                automaticLayout: true,
                            }}
                        />
                    </div>
                </Card>
                <Card className="p-4 rounded-lg">
                    <div className="flex justify-between">
                        <p className="font-bold">Test Cases</p>
                        <div className="flex gap-2">
                            {formattedTestCases.map((tc, index) => (
                                <button
                                    key={index}
                                    onClick={() => setSelectedTestCase(index)}
                                    className={`w-8 h-8 rounded-lg text-white flex items-center justify-center 
                    ${
                        tc.status === "success"
                            ? "bg-green-500"
                            : tc.status === "error"
                            ? "bg-red-500"
                            : "bg-gray-500"
                    } 
                    ${
                        selectedTestCase === index ? "ring-2 ring-blue-500" : ""
                    }`}
                                >
                                    {index + 1}
                                </button>
                            ))}
                        </div>
                    </div>

                    {displayedTestCases?.map((testCase, index) => (
                        <div
                            key={index}
                            className="grid grid-cols-3 gap-4 mt-3"
                        >
                            <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg">
                                {testCase?.input || "No input provided"}
                            </div>
                            <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg">
                                {testCase?.output || "No output provided"}
                            </div>
                        </div>
                    ))}
                </Card>
            </div>
        </div>
    );
}
