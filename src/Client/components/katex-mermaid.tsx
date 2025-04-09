'use client'

import React, { useState, useRef, useEffect, Fragment, useCallback } from 'react'
import MDEditor from '@uiw/react-md-editor'
import mermaid from 'mermaid'
import katex from 'katex'
import 'katex/dist/katex.css'
import rehypeSanitize from 'rehype-sanitize'

const mdMixed = `
This is to display the KaTeX in one line:
\`\$\$\c = \\pm\\sqrt{a^2 + b^2}\$\$\`

\`\`\`KaTeX
c = \\pm\\sqrt{a^2 + b^2}
\`\`\`

And here's a Mermaid diagram:

\`\`\`mermaid
graph TD
A[Hard] -->|Text| B(Round)
B --> C{Decision}
C -->|One| D[Result 1]
C -->|Two| E[Result 2]
\`\`\`
`

const randomid = () => parseInt(String(Math.random() * 1e15), 10).toString(36)

interface CodeProps {
  children?: React.ReactNode;
  className?: string;
}

const Code: React.FC<CodeProps> = ({ children = [], className = '' }) => {
  const demoid = useRef(`dome${randomid()}`);
  const [container, setContainer] = useState<HTMLElement | null>(null);
  const code = Array.isArray(children)
    ? children.map((child) => (typeof child === 'string' ? child : '')).join('')
    : typeof children === 'string'
    ? children
    : '';

  useEffect(() => {
    if (container && code) {
      const isMermaid = className && /^language-mermaid/.test(className.toLowerCase());
      const isKaTeX = className && /^language-katex/.test(className.toLowerCase());

      if (isMermaid) {
        mermaid
          .render(demoid.current, code)
          .then(({ svg, bindFunctions }) => {
            container.innerHTML = svg;
            if (bindFunctions) {
              bindFunctions(container);
            }
          })
          .catch((error) => console.log('mermaid error:', error));
      } else if (isKaTeX) {
        const html = katex.renderToString(code, { throwOnError: false });
        container.innerHTML = html;
      }
    }
  }, [container, className, code]);

  const refElement = useCallback((node: HTMLElement | null) => {
    if (node !== null) {
      setContainer(node);
    }
  }, []);

  if (className && /^language-mermaid/.test(className.toLowerCase())) {
    return (
      <Fragment>
        <code id={demoid.current} style={{ display: 'none' }} />
        <code className={className} ref={refElement} data-name="mermaid" />
      </Fragment>
    );
  }

  if (className && /^language-katex/.test(className.toLowerCase())) {
    return <code className={className} ref={refElement} />;
  }

  if (typeof children === 'string' && /^\$\$(.*)\$\$/.test(children)) {
    const html = katex.renderToString(children.replace(/^\$\$(.*)\$\$/, '$1'), { throwOnError: false });
    return <code dangerouslySetInnerHTML={{ __html: html }} style={{ background: 'transparent' }} />;
  }

  return <code className={className}>{children}</code>;
};

export default function MdEditor() {
  const [value, setValue] = useState(mdMixed)

  return (
    <MDEditor
      className="w-full"
      onChange={(newValue = '') => setValue(newValue)}
      textareaProps={{
        placeholder: 'Please enter Markdown text',
      }}
      height={500}
      value={value}
      previewOptions={{
        rehypePlugins: [[rehypeSanitize]],
        components: {
          code: (props) => <Code {...props} />,
        },
      }}
    />
  )
}
