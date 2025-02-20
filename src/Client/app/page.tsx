"use client"
import '../styles/globals.css'

export default function Component(){

    return(
        <>
            <div className="flex justify-center items-center w-full gap-10 h-screen text-xl">
                <div className={"flex flex-col gap-5 ml-7"}>
                    <h2 className={" text-5xl font-bold bg-gradient-to-r from-primary to-danger bg-clip-text text-transparent"}>Online
                        Proctoring System</h2>
                    <p className={"text-white text-sm border-4 border-double rounded-r-full bg-blue-800 p-3"}>A digital
                        method of monitoring exams that ensures test integrity and
                        prevents cheating conducted via online software that enables students to sit for tests from any
                        location.</p>
                </div>
                <img src="https://i.ibb.co.com/m5C10Wmb/ops-banner-removebg-preview.png"
                     alt="ops-banner-removebg-preview" className={"mt-12 w-[700px] h-[400px]"}/>
            </div>
        </>

    )
}