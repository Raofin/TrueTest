"use client"
import {
    Button,
    Card,
    Pagination,
    Table,
    TableBody,
    TableCell,
    TableColumn,
    TableHeader,
    TableRow, SortDescriptor,  Selection,
} from "@heroui/react";
import '../../styles/globals.css'
import CountUp from 'react-countup';
import {Chip, cn} from "@nextui-org/react";
import {Icon} from "@iconify/react";
import React, {SVGProps} from "react";

import {useRouter} from "next/navigation";
type TrendCardProps = {
    title: string;
    value: number;
    change: string;
    changeType: "positive" | "neutral" | "negative";
    trendType: "up" | "neutral" | "down";
    trendChipPosition?: "top" | "bottom";
    trendChipVariant?: "flat" | "light";
};

const data: TrendCardProps[] = [  {
    title: "Total Exam",
    value: 2284,
    change: "33%",
    changeType: "positive",
    trendType: "up",
},
    {
        title: "Completed Reviewes",
        value: 718,
        change: "13.0%",
        changeType: "negative",
        trendType: "up",
    },
    {
        title: "Flagged Event",
        value: 150,
        change: "0.0%",
        changeType: "neutral",
        trendType: "neutral",
    },
];
const TrendCard = ({
                       title,
                       value,
                       change,
                       changeType,
                       trendType,
                       trendChipPosition = "top",
                       trendChipVariant = "light",
                   }: TrendCardProps) => {
    return (
        <Card className=" border border-transparent dark:border-default-100 mt-12 ml-8">
            <div className="flex p-4">
                <div className="flex flex-col gap-y-2">
                    <dt className="text-small font-medium text-default-500">{title}</dt>
                    <dd className="text-2xl font-semibold text-default-700"><CountUp start={0} duration={2} end={value} /></dd>
                </div>
                <Chip
                    className={cn("absolute right-4", {
                        "top-4": trendChipPosition === "top",
                        "bottom-4": trendChipPosition === "bottom",
                    })}
                    classNames={{
                        content: "font-medium text-[0.65rem]",
                    }}
                    color={
                        changeType === "positive" ? "success" : changeType === "neutral" ? "warning" : "danger"
                    }
                    radius="sm"
                    size="sm"
                    startContent={
                        trendType === "up" ? (
                            <Icon height={12} icon={"solar:arrow-right-up-linear"} width={12} />
                        ) : trendType === "neutral" ? (
                            <Icon height={12} icon={"solar:arrow-right-linear"} width={12} />
                        ) : (
                            <Icon height={12} icon={"solar:arrow-right-down-linear"} width={12} />
                        )
                    }
                    variant={trendChipVariant}>
                    {change}
                </Chip>
            </div>
        </Card>
    );
};
export type IconSvgProps = SVGProps<SVGSVGElement> & {
    size?: number;
};


const SearchIcon = (props: IconSvgProps) => {
    return (
        <svg
            aria-hidden="true"
            fill="none"
            focusable="false"
            height="1em"
            role="presentation"
            viewBox="0 0 24 24"
            width="1em"
            {...props}
        >
            <path
                d="M11.5 21C16.7467 21 21 16.7467 21 11.5C21 6.25329 16.7467 2 11.5 2C6.25329 2 2 6.25329 2 11.5C2 16.7467 6.25329 21 11.5 21Z"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
            />
            <path
                d="M22 22L20 20"
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
            />
        </svg>
    );
};

const columns = [
    {name: "Exam ID", uid: "ExamId"},
    {name: "Exam Name", uid: "ExamName"},
    {name: "Total Time", uid: "TotalTime"},
    {name: "Total Score", uid: "TotalScore"},
    {name: "Date", uid: "Date",sortable: true},
    {name: "Start Time", uid: "StartTime"},
    {name:"Status",uid:"Status",sortable: true},
];
const users = [{
    ExamId: 1,
    ExamName: "QA engineer intern",
    TotalTime:"2hr",
    TotalScore:"100",
    Date:"23-02-2025",
    StartTime:"08:20",
    Status:"Review",
},{
    ExamId: 2,
    ExamName: "QA engineer intern",
    TotalTime:"2hr",
    TotalScore:"100",
    Date:"25-02-2025",
    StartTime:"08:20",
    Status:"Reviewed",
},{
    ExamId: 3,
    ExamName: "QA engineer intern",
    TotalTime:"2hr",
    TotalScore:"100",
    Date:"26-02-2025",
    StartTime:"08:20",
    Status:"Review",
}
];
const INITIAL_VISIBLE_COLUMNS = ["ExamId","ExamName","TotalTime","TotalScore","Date","StartTime","Status"];
type User = (typeof users)[0];
export default function Component(){
    const [filterValue, setFilterValue] = React.useState("");
    const [selectedKeys, setSelectedKeys] = React.useState<Selection>(new Set([]));
    const [visibleColumns, setVisibleColumns] = React.useState<Selection>(
        new Set(INITIAL_VISIBLE_COLUMNS),
    );
    const router=useRouter();
    const handlereview=(ExamId:number)=>{
        router.push(`/reviewer_dashboard/exam-review/${ExamId}`);
    }
    const [statusFilter, setStatusFilter] = React.useState<Selection>("all");
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [sortDescriptor, setSortDescriptor] = React.useState<SortDescriptor>({
        column:"Date",
        direction: "ascending",
    });
    const [page, setPage] = React.useState(1);
    const hasSearchFilter = Boolean(filterValue);
    const headerColumns = React.useMemo(() => {
        if (visibleColumns === "all") return columns;

        return columns.filter((column) => Array.from(visibleColumns).includes(column.uid));
    }, [visibleColumns]);
    const filteredItems = React.useMemo(() => {
        let filteredUsers = [...users];

        if (hasSearchFilter) {
            filteredUsers = filteredUsers.filter((user) =>
                user.ExamName.toLowerCase().includes(filterValue.toLowerCase()),
            );
        }
        return filteredUsers;
    }, [users, filterValue, statusFilter]);
    const pages = Math.ceil(filteredItems.length / rowsPerPage);
    const items = React.useMemo(() => {
        const start = (page - 1) * rowsPerPage;
        const end = start + rowsPerPage;
        return filteredItems.slice(start, end);
    }, [page, filteredItems, rowsPerPage]);
    const sortedItems = React.useMemo(() => {
        return [...items].sort((a: User, b: User) => {
            const first = a[sortDescriptor.column as keyof User] as number;
            const second = b[sortDescriptor.column as keyof User] as number;
            const cmp = first < second ? -1 : first > second ? 1 : 0;

            return sortDescriptor.direction === "descending" ? -cmp : cmp;
        });
    }, [sortDescriptor, items]);
    const renderCell = React.useCallback((user: User, columnKey: React.Key,ExamId:number) => {
        const cellValue = user[columnKey as keyof User];
        switch (columnKey) {
            case "Status":
                return(
                    <div>
                        {cellValue==='Review'?<Button onPress={()=>handlereview(ExamId)} radius="full" className={'text-white text-md bg-primary'}>Review</Button>
                        :<Chip color="default"  className={'text-white text-md'}>Reviewed</Chip>}
                    </div>
                );
            default:
                return cellValue;
        }
    }, []);
    const onNextPage = React.useCallback(() => {
        if (page < pages) {
            setPage(page + 1);
        }
    }, [page, pages]);
    const onPreviousPage = React.useCallback(() => {
        if (page > 1) {
            setPage(page - 1);
        }
    }, [page]);
    const onRowsPerPageChange = React.useCallback((e: React.ChangeEvent<HTMLSelectElement>) => {
        setRowsPerPage(Number(e.target.value));
        setPage(1);
    }, []);
    const onSearchChange = React.useCallback((value?: string) => {
        if (value) {
            setFilterValue(value);
            setPage(1);
        } else {
            setFilterValue("");
        }
    }, []);

    const topContent = React.useMemo(() => {
        return (
            <div className="flex flex-col gap-4 mt-8 w-full">
                <div className="flex justify-between items-center">
                    <span className="text-default-400 text-small">Total {users.length} users</span>
                    <label className="flex items-center text-default-400 text-small">
                        Rows per page:
                        <select
                            className="bg-transparent outline-none text-default-400 text-small"
                            onChange={onRowsPerPageChange}>
                            <option value="5">5</option>
                            <option value="10">10</option>
                            <option value="15">15</option>
                        </select>
                    </label>
                </div>
            </div>
        );
    }, [
        filterValue,
        statusFilter,
        visibleColumns,
        onSearchChange,
        onRowsPerPageChange,
        users.length,
        hasSearchFilter,
    ]);
    const bottomContent = React.useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
        <span className="w-[30%] text-small text-default-400">
          {selectedKeys === "all"
              ? "All items selected"
              : `${selectedKeys.size} of ${filteredItems.length} selected`}
        </span>
                <Pagination
                    isCompact
                    showControls
                    showShadow
                    color="primary"
                    page={page}
                    total={pages}
                    onChange={setPage}
                />
                <div className="hidden sm:flex w-[30%] justify-end gap-2">
                    <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onPreviousPage}>
                        Previous
                    </Button>
                    <Button isDisabled={pages === 1} size="sm" variant="flat" onPress={onNextPage}>
                        Next
                    </Button>
                </div>
            </div>
        );
    }, [selectedKeys, items.length, page, pages, hasSearchFilter]);
    return(
        <>
            <dl className="grid w-full grid-cols-1 gap-5 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4">
                {data.map((props, index) => (
                    <TrendCard key={index} {...props} />
                ))}
            </dl>
            <Table className="px-10 mx-2"
                   isHeaderSticky
                   aria-label="Example table with custom cells, pagination and sorting"
                   bottomContent={bottomContent}
                   bottomContentPlacement="outside"
                   classNames={{
                       wrapper: "max-h-[382px]",
                   }}
                   sortDescriptor={sortDescriptor}
                   topContent={topContent}
                   topContentPlacement="outside"
                   onSortChange={setSortDescriptor}>
                <TableHeader columns={headerColumns}>
                    {(column) => (
                        <TableColumn
                            key={column.uid}
                            align={ "center"} className={" font-semibold"}
                            allowsSorting={column.sortable}>
                            {column.name}
                        </TableColumn>
                    )}
                </TableHeader>
                <TableBody emptyContent={"No exam found"} items={sortedItems}>
                    {(item) => (
                        <TableRow key={item.ExamId}>
                            {(columnKey) => <TableCell>{renderCell(item, columnKey,item.ExamId)}</TableCell>}
                        </TableRow>
                    )}
                </TableBody>
            </Table>
        </>

    )
}