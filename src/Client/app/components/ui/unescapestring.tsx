'use client'

export default function unescapeJsonString(str:string) {
  return str.replace(/\\(.)/g, function (match, capturedChar) {
    switch (capturedChar) {
      case '"':
        return '"';
      case '\\':
        return '\\';
      case '/':
        return '/';
      case 'b':
        return '\b';
      case 'f':
        return '\f';
      case 'n':
        return '\n';
      case 'r':
        return '\r';
      case 't':
        return '\t';
      default:
        return capturedChar;
    }
  });
}