"use client"
import React from 'react'
import {useParams, useRouter} from "next/navigation";

const Verification=()=>{
    const  router=useRouter();
    const param=useParams<{username:string}>()

    return(
        <div>Verify Account</div>
    )
}