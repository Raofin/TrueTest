'use client'

export default function escapeJsonString(str:string) {
  return str.replace(/[\"\\\/\b\f\n\r\t]/g, function (char) {
    switch (char) {
      case '"':
        return '\\"';
      case '\\':
        return '\\\\';
      case '/':
        return '\\/';
      case '\b':
        return '\\b';
      case '\f':
        return '\\f';
      case '\n':
        return '\\n';
      case '\r':
        return '\\r';
      case '\t':
        return '\\t';
      default:
        return char;
    }
  });
}