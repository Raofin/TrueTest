"use client"
import '../styles/globals.css'

export default function Component(){

    return(
        <>
            <div className="flex justify-center items-center w-full gap-10  text-xl">
                <div className={"flex flex-col gap-5 ml-7"}>
                    <h2 className={" text-5xl font-bold bg-gradient-to-r from-primary to-danger bg-clip-text text-transparent"}>Online Proctoring System</h2>
                    <p className={"text-sm"}>A digital method of monitoring exams that ensures test integrity and
                        prevents cheating conducted via online software that enables students to sit for tests from any
                        location.</p>
                    {/*<img className="mt-12 ml-10" src="https://i.ibb.co.com/KzXFQHXq/freepik-background-45058.png"*/}
                    {/*   width={"200px"} height={"200px"}  alt="freepik-background-45058"*/}
                    {/*     />*/}
                </div>

                <img
                    src="https://i.ibb.co.com/3yPywyT1/Online-Proctoring-Software-info-l.webp"
                    alt="Online-Proctoring-Software-info-l" className={"mt-12"} />
            </div>
        </>

    )
}