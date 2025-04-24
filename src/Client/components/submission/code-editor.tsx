"use client";

import React, { useEffect, useState } from "react";
import { Button, Card, Select, SelectItem } from "@heroui/react";
import Editor from "@monaco-editor/react";
import api from "@/lib/api";
import { languages } from "@/lib/language-selector";

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
    readonly question: ProblemQuestion;
    readonly setAnswers: React.Dispatch<
        React.SetStateAction<{ [key: string]: string | string[] }>
    >;
    readonly questionId: string;
    readonly examId: string;
}

interface CodeState {
    [key: string]: string;
}
export default function CodeEditor({
    question,
    setAnswers,
    questionId,
    examId
}: PageProps) {
    const [selectedLanguage, setSelectedLanguage] = useState("cpp");
    const [codeStates, setCodeStates] = React.useState<CodeState>({});
    const [selectedTestCase, setSelectedTestCase] = useState<number>(0);
    const [formattedTestCases, setFormattedTestCases] = useState<TestCase[]>([]);
    const [displayTestCase, setDisplayTestCase] = useState<boolean>(false);
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
            output:tc.output||"Error",
            status: "pending" as const,
        }));
        setFormattedTestCases(initializedTestCases);
        console.log(initializedTestCases)
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
        const currentCode = codeStates[selectedLanguage];
        const currentInput = question.testCases[selectedTestCase]?.input ?? "";
    
        const payloadOfAnyCode = {
            code: currentCode,
            input: currentInput,
            languageId: selectedLanguage,
        };
    
        try {
            const runResponse = await api.post('/Candidate/RunAnyCode', payloadOfAnyCode);
            if (runResponse.status === 200) {
                const runData = await runResponse.data;
                console.log(runData);
                const newReceivedOutput = runData.stdout ?? runData.stderr ?? runData.exception ?? "No output";
                const newStatus = runData.status === "success" ? "success" : "error" as TestCase["status"];
    
                const updatedTestCasesAfterRun = formattedTestCases.map((tc, index) => {
                    if (index === selectedTestCase) {
                        return {
                            ...tc,
                            output:tc.output ||"Error",
                            receivedOutput: newReceivedOutput,
                            status: newStatus,
                        };
                    }
                    return tc;
                });
                setFormattedTestCases(updatedTestCasesAfterRun);
                setDisplayTestCase(true);
                const savePayload = {
                    examId: examId,
                    questionId: question.questionId,
                    code: currentCode,
                    language: selectedLanguage,
                };
                try {
                    const saveResponse = await api.put("/Candidate/Submit/Problem/Save", savePayload);
                    if (saveResponse.status === 200) {
                        const saveData = await saveResponse.data;
                        console.log(saveData);
                        const updatedWithSave = updatedTestCasesAfterRun.map((testCase) => {
                            const result = saveData.find(
                                (r: any) => r.testCaseId === testCase.testCaseId
                            );
                            let status = testCase.status;
                            let output = testCase.output;
    
                            if (result?.isAccepted) status = "success";
                            else if (result?.isAccepted === false) status = "error";
                            if (result?.output) output = result.output;     
                            return {
                                ...testCase,
                                output: output ?? "",
                                status: status as "success" | "error" | "pending",
                            };
                        });
                        setFormattedTestCases(updatedWithSave);
                    } else {
                        console.error("Failed to save code");
                    }
                } catch (saveError) {
                    console.error("Error during code save:", saveError);
                }
            } else {
                console.error("Failed to run code");
            }
        } catch (runError) {
            console.error("Error during code execution:", runError);
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
                        <h3 className="font-semibold">Example</h3>
                        {question.testCases.map((testCase) => (
                            <div
                                key={testCase.input}
                                className="mt-2 p-3 rounded-lg"
                            >
                                <div>
                                    <span className="font-semibold">
                                        Input:
                                    </span>
                                    {testCase.input}
                                </div>
                                <div>
                                    <span className="font-semibold">
                                        Output:
                                    </span>
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
                                onChange={(e) =>
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
                {displayTestCase && <Card className="p-4 rounded-lg">
                    <div className="flex justify-between">
                        <p className="font-bold">Test Cases</p>
                        <div className="flex gap-2">
                            {formattedTestCases.map((tc, index) => {
                                const getStatusColor = () => {
                                    if (tc.status === "success")
                                        return "bg-green-500";
                                    if (tc.status === "error")
                                        return "bg-red-500";
                                    return "bg-gray-500";
                                };
                                const isSelected =
                                    selectedTestCase === index
                                        ? "ring-2 ring-blue-500"
                                        : "";
                                return (
                                    <button
                                        key={index}
                                        onClick={() =>
                                            setSelectedTestCase(index)
                                        }
                                        className={`w-8 h-8 rounded-lg text-white flex items-center justify-center ${getStatusColor()} ${isSelected}`}
                                    >
                                        {index + 1}
                                    </button>
                                );
                            })}
                        </div>
                    </div>

                    <div className='flex w-full justify-between mt-4'>
                        <p className="font-semibold">Input</p>
                        <p className="font-semibold">Received Output</p>
                        <p className="font-semibold">Expected Output</p>
                    </div>
                    {displayedTestCases?.map((testCase) => (
                        <div
                            key={testCase?.input}
                            className="grid grid-cols-3 gap-4 mt-3 min-h-[100px]">
                            <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg whitespace-pre-wrap">
                                {testCase?.input ?? "No input provided"}
                            </div>
                            <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg whitespace-pre-wrap">
                                {testCase?.receivedOutput ?? "No output yet"}
                            </div>
                            <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg whitespace-pre-wrap">
                                {testCase?.output ?? "No expected output"}
                            </div>
                        </div>
                    ))}
                </Card>}
            </div>
        </div>
    );
}