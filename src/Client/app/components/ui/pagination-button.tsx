import React from "react";
import {Button } from "@heroui/react";
interface PageProps{
    currentIndex:number;
    totalItems:number;
    onPrevious:()=>void
    onNext:()=>void
}
const PaginationButtons = ({ currentIndex, totalItems, onPrevious, onNext }:PageProps) => {
  return (
    <div className="flex gap-2">
      <Button
        size="sm"
        isDisabled={currentIndex <= 1} 
        onPress={onPrevious}>
        Previous
      </Button>
      <Button
        size="sm"
        isDisabled={currentIndex >= totalItems} 
        onPress={onNext}>
        Next
      </Button>
    
    </div>
  );
};

export default PaginationButtons;
