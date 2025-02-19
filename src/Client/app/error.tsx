'use client'
import React from 'react'
import {Button} from "@nextui-org/react";
interface Props{
    error:Error;
    reset:()=>void;
}
const Error=({error,reset}:Props)=>{
   console.log('Error ',error);
    return(
        <>
          <p>An unexpected error has occured.</p>
            <Button onPress={()=>reset()}>Reload this page</Button>
        </>
    )
}
export default Error;