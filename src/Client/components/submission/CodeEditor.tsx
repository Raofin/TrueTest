"use client";

import React, { useEffect, useState } from "react";
import { Button, Card, Select, SelectItem, Spinner } from "@heroui/react";
import Editor from "@monaco-editor/react";
import api from "@/lib/api";
import { languages } from "@/lib/language-selector";
import { ProblemQuestion, TestCase } from '@/components/types/problemQues'
import ReactMarkdown from "react-markdown";

interface PageProps {
    readonly question: ProblemQuestion;
    readonly setAnswers: React.Dispatch<
        React.SetStateAction<{ [key: string]: string | string[] }>
    >;
    readonly answers: { [key: string]: string | string[] };
    readonly questionId: string;
    readonly examId: string;
    readonly persistedTestCaseResults?: TestCase[];
    readonly onTestCaseRunComplete: (questionId: string, results: TestCase[]) => void;
}
interface CodeState {
    [key: string]: string;
}

export default function CodeEditor({
    question,
    setAnswers,
    questionId,
    answers,
    examId,
    persistedTestCaseResults,
    onTestCaseRunComplete,
}: PageProps) {
    const [selectedLanguage, setSelectedLanguage] = useState("cpp");
    const [codeStates, setCodeStates] = React.useState<CodeState>({});
    const [selectedTestCase, setSelectedTestCase] = useState<number>(0);
    const [formattedTestCases, setFormattedTestCases] = useState<TestCase[]>([]);
    const [displayTestCaseResults, setDisplayTestCaseResults] = useState<boolean>(false);
    const displayedTestCasesResults = [formattedTestCases[selectedTestCase]];
    const [loading,setLoading]=useState(false);
    useEffect(() => {
        const initialStates: CodeState = {};
        languages.forEach((lang) => {
            initialStates[lang.value] = lang.defaultCode;
        });
        const existingCode = answers[questionId] as string | undefined;
        if (existingCode) {
            setCodeStates({ [selectedLanguage]: existingCode });
        } else {
            setCodeStates(initialStates);
        }
        if (persistedTestCaseResults) {
            setFormattedTestCases(persistedTestCaseResults);
            setDisplayTestCaseResults(true);
        } else {
            const initializedTestCases = question.testCases.map((tc) => ({
                ...tc,
                receivedOutput: "",
                output: tc.output || "Error",
                status: "pending" as const,
                executionTime:0,
                errorMessage:""
            }));
            setFormattedTestCases(initializedTestCases);
        }
    }, [question.testCases, questionId, answers, persistedTestCaseResults, selectedLanguage]);

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
    const handleLanguageChange = (newLanguage: string) => {
        setSelectedLanguage(newLanguage);
    };
    console.log(displayedTestCasesResults)
    const handleRun = async () => {
        setLoading(true)
        const currentCode = codeStates[selectedLanguage];
        const savePayload = {
            examId: examId,
            questionId: question.questionId,
            code: currentCode,
            language: selectedLanguage,
        };
        try {
            const saveResponse = await api.put("/Candidate/Submit/Problem/Save", savePayload);
            if (saveResponse.status === 200) {
                const responseData: {
                    testCaseId: string;
                    isAccepted: boolean;
                    receivedOutput: string;
                    errorMessage: string;
                    exception: string;
                    executionTime: number;
                }[] = await saveResponse.data;

                const updatedTestCases = formattedTestCases.map((tc) => {
                    const matchingResult = responseData.find(
                        (res) => res.testCaseId === tc.testCaseId
                    );
                    return {
                        ...tc,
                        receivedOutput: matchingResult?.receivedOutput,
                        status: matchingResult
                            ? matchingResult.isAccepted
                                ? "success"
                                : "error"
                            : tc.status,
                        errorMessage: matchingResult?.errorMessage
                    } as TestCase;

                });
                setFormattedTestCases(updatedTestCases);
                setDisplayTestCaseResults(true);
                onTestCaseRunComplete(questionId, updatedTestCases);
            } else {
                console.error("Failed to save code");
            }
        } catch (saveError) {
            console.error("Error during code save:", saveError);
        }finally{ setLoading(false)}
    };

    return (
        <div>
        <div className="grid grid-cols-2 gap-4">
            <Card className="border-none rounded-lg p-4 shadow-none bg-white dark:bg-[#18181b]">
                <h2 className="text-xl font-bold mb-3">Problem Statement</h2>
                <div className="space-y-4">
                    <ReactMarkdown>{question.statementMarkdown}</ReactMarkdown>
                </div>
            </Card>
            <div>
                <Card className="px-3 rounded-lg h-[500px] mb-3">
                    <div className="flex justify-between pt-2">
                        <p className="font-semibold">Code Editor</p>
                        <div className="flex gap-2">
                           {loading?<Button><Spinner/></Button>: <Button onPress={handleRun}>Run</Button>}
                            <Select
                                aria-label="Select Language"
                                selectedKeys={[selectedLanguage]}
                                onChange={(e) => handleLanguageChange(e.target.value)}
                                className="w-40"
                            >
                                {languages.map((lang) => (
                                    <SelectItem key={lang.value}>{lang.label}</SelectItem>
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
                {displayTestCaseResults && (
                    <Card className="p-4 rounded-lg">
                        <div className="flex justify-between">
                            <p className="font-bold">Test Cases</p>
                            <div className="flex gap-2">
                                {formattedTestCases.map((tc, index) => {
                                    const getStatusColor = () => {
                                        if (tc.status === "success") return "bg-green-500";
                                        if (tc.status === "error") return "bg-red-500";
                                        return "bg-gray-500";
                                    };
                                    const isSelected = selectedTestCase === index ? "ring-2 ring-blue-500" : "";
                                    return (
                                        <button
                                            key={index}
                                            onClick={() => setSelectedTestCase(index)}
                                            className={`w-8 h-8 rounded-lg text-white flex items-center justify-center ${getStatusColor()} ${isSelected}`}
                                        >
                                            {index + 1}
                                        </button>
                                    );
                                })}
                            </div>
                        </div>
                        {displayedTestCasesResults?.map((testCase) => (<>
                        <div key={testCase?.input} >
                        <div className="flex w-full justify-between mt-4">
                            <p className="font-semibold">Input</p>
                           <p className="font-semibold">Received Output <span className='text-gray-700'>{testCase.executionTime}</span></p>
                            <p className="font-semibold">Expected Output</p>
                        </div>
                            <div className="grid grid-cols-3 gap-4 mt-3 min-h-[200px]">
                                <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg whitespace-pre-wrap">
                                    {testCase?.input ?? "No input provided"}
                                </div>
                               <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg whitespace-pre-wrap">
                                    {testCase.status==="error" ? testCase?.errorMessage : testCase?.receivedOutput}
                                </div>
                                <div className="font-mono p-2 bg-[#f4f4f5] dark:bg-[#27272a] rounded-lg whitespace-pre-wrap">
                                    {testCase?.output ?? "No expected output"}
                                </div>
                            </div>
                            </div>
                       </> ))}
                    </Card>
                )}
            </div>
        </div>
        </div>
    );
}