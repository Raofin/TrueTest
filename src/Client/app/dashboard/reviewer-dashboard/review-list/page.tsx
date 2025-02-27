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

import CountUp from 'react-countup';
import {Chip} from "@nextui-org/react";
import React from "react";

import {useRouter} from "next/navigation";
type TrendCardProps = {
    title: string;
    value: number;
};

const data: TrendCardProps[] = [  {
    title: "Total Exam",
    value: 2284,
},
    {
        title: "Completed Reviewes",
        value: 718,
    },
    {
        title: "Flagged Event",
        value: 150,
    },
];
const TrendCard = ({
                       title,
                       value
                   }: TrendCardProps) => {
    return (
        <Card className=" border border-transparent dark:border-default-100 mt-2 ml-8">
            <div className="flex p-4">
                <div className="flex flex-col gap-y-2">
                    <dt className="text-small font-medium text-default-500">{title}</dt>
                    <dd className="text-2xl font-semibold text-default-700"><CountUp start={0} duration={2} end={value} /></dd>
                </div>

            </div>
        </Card>
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
    const filterValue = "";
  
    const [visibleColumns, setVisibleColumns] = React.useState<Selection>(
        new Set(INITIAL_VISIBLE_COLUMNS),
    );
    const router=useRouter();
    const handlereview = React.useCallback((ExamId: number) => {
        router.push(`/reviewer_dashboard/exam-review/${ExamId}`);
    }, [router]);
 
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [sortDescriptor, setSortDescriptor] = React.useState<SortDescriptor>({
        column:"Date",
        direction: "ascending",
    });
    const [page, setPage] = React.useState(1);
    const hasSearchFilter = Boolean(filterValue);
    const headerColumns = React.useMemo(() => {
        if (visibleColumns === "all") {setVisibleColumns("all") ; return columns;}

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
    }, [ filterValue,hasSearchFilter]);
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
    }, [handlereview]);
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

    const topContent = React.useMemo(() => {
        return (
            <div className="flex flex-col gap-4 mt-8 w-full ">
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
        onRowsPerPageChange,
    ]);
    const bottomContent = React.useMemo(() => {
        return (
            <div className="py-2 px-2 flex justify-between items-center">
        <span className="w-[30%] text-small text-default-400">
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
    }, [ page, pages,onNextPage,onPreviousPage]);
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
                   classNames={{wrapper: "max-h-[382px]"}}
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