"use client";

import React, { useState } from "react";
import Image from "next/image";
import { Package } from "lucide-react";

interface SafeImageProps {
  src: string;
  alt: string;
  width?: number;
  height?: number;
  fill?: boolean;
  className?: string;
  sizes?: string;
}

export function SafeImage({
  src,
  alt,
  width,
  height,
  fill,
  className,
  sizes,
}: SafeImageProps) {
  const [imageError, setImageError] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  if (imageError || !src) {
    return (
      <div
        className={`flex items-center justify-center bg-gray-100 text-gray-400 ${className}`}
      >
        <Package className="h-12 w-12" />
      </div>
    );
  }

  return (
    <>
      {isLoading && (
        <div
          className={`flex items-center justify-center bg-gray-100 text-gray-400 ${className}`}
        >
          <div className="animate-pulse">
            <Package className="h-12 w-12" />
          </div>
        </div>
      )}
      <Image
        src={src}
        alt={alt}
        width={width}
        height={height}
        fill={fill}
        className={`${className} ${
          isLoading ? "opacity-0" : "opacity-100"
        } transition-opacity duration-300`}
        sizes={sizes}
        onError={() => setImageError(true)}
        onLoad={() => setIsLoading(false)}
        unoptimized={src.includes("placeholder") || src.includes("picsum")}
      />
    </>
  );
}
