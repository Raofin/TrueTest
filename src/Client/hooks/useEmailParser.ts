import { useState } from 'react';
import Papa from 'papaparse';
import { toast } from 'react-hot-toast';
import isValidEmail from '@/components/check-valid-email';

interface User {
  email: string;
}

interface CsvRow {
  email?: string;
  [key: string]: unknown;
}

interface EmailParserResult {
  userEmails: User[];
  parseEmailContent: (content: string) => void;
  setUserEmails: React.Dispatch<React.SetStateAction<User[]>>;
}

export const useEmailParser = (initialEmails: User[] = []): EmailParserResult => {
  const [userEmails, setUserEmails] = useState<User[]>(initialEmails);

  const parseEmailContent = (content: string) => {
    const trimmedContent = content.trim();

    if (!trimmedContent) {
      toast.error('Please enter or paste email addresses');
      return;
    }

    if (!trimmedContent.includes(',') && !trimmedContent.includes('\n')) {
      const email = trimmedContent;
      if (!isValidEmail(email)) {
        toast.error(`Invalid email: ${email}`);
        return;
      }

      if (userEmails.some((e) => e.email === email)) {
        toast.error(`Email already exists: ${email}`);
        return;
      }

      setUserEmails([{ email }]);
      toast.success('Email successfully added');
      return;
    }

    const isLikelyCSV = () => {
      const lines = content.split('\n');
      if (lines.length < 2) return false;

      const firstLine = lines[0];
      const secondLine = lines[1];
      const hasHeaders = firstLine.includes(',');
      const firstLineCommas = firstLine.split(',').length;
      const secondLineCommas = secondLine.split(',').length;

      return hasHeaders && firstLineCommas > 1 && firstLineCommas === secondLineCommas;
    };

    if (isLikelyCSV()) {
      Papa.parse(content, {
        header: true,
        skipEmptyLines: true,
        complete: (result) => {
          const { validEmails, invalidEmails, duplicateEmails } = processCsvData(result.data);
          handleEmailResults(validEmails, invalidEmails, duplicateEmails);
        },
        error: () => {
          const { validEmails, invalidEmails, duplicateEmails } = processEmailList(trimmedContent);
          handleEmailResults(validEmails, invalidEmails, duplicateEmails);
        },
      });
    } else {
      const { validEmails, invalidEmails, duplicateEmails } = processEmailList(trimmedContent);
      handleEmailResults(validEmails, invalidEmails, duplicateEmails);
    }
  };

  const processCsvData = (data: unknown[]) => {
    const validEmails: User[] = [];
    const invalidEmails: string[] = [];
    const duplicateEmails: string[] = [];
    const existingEmails = new Set(userEmails.map((u) => u.email.toLowerCase()));
    const rows = data as CsvRow[];
    const startIndex = rows[0]?.email?.toLowerCase().includes('email') ? 1 : 0;

    for (let i = startIndex; i < rows.length; i++) {
      const row = rows[i];
      const emailValue = row.email ?? Object.values(row)[0];
      const email = (emailValue || '').toString().trim();
      if (!email) continue;

      const emailLower = email.toLowerCase();

      if (!isValidEmail(email)) {
        invalidEmails.push(email);
        continue;
      }

      if (existingEmails.has(emailLower)) {
        duplicateEmails.push(email);
        continue;
      }

      existingEmails.add(emailLower);
      validEmails.push({ email });
    }

    return { validEmails, invalidEmails, duplicateEmails };
  };

  const processEmailList = (content: string) => {
    const emails = content.split(/[\n,]+/);
    const validEmails: User[] = [];
    const invalidEmails: string[] = [];
    const duplicateEmails: string[] = [];
    const existingEmails = new Set(userEmails.map((u) => u.email.toLowerCase()));

    emails.forEach((email) => {
      const trimmedEmail = email.trim();
      if (!trimmedEmail) return;

      const emailLower = trimmedEmail.toLowerCase();

      if (!isValidEmail(trimmedEmail)) {
        invalidEmails.push(trimmedEmail);
        return;
      }

      if (existingEmails.has(emailLower)) {
        duplicateEmails.push(trimmedEmail);
        return;
      }

      existingEmails.add(emailLower);
      validEmails.push({ email: trimmedEmail });
    });

    return { validEmails, invalidEmails, duplicateEmails };
  };

  const handleEmailResults = (validEmails: User[], invalidEmails: string[], duplicateEmails: string[]) => {
    if (invalidEmails.length > 0) {
      const invalidCount = invalidEmails.length;
      const displayInvalid =
        invalidCount > 5
          ? `${invalidEmails.slice(0, 5).join(', ')} and ${invalidCount - 5} more...`
          : invalidEmails.join(', ');

      toast.error(`Found ${invalidCount} invalid email(s): ${displayInvalid}`, {
        duration: 5000,
      });
    }

    if (duplicateEmails.length > 0) {
      const duplicateCount = duplicateEmails.length;
      const displayDuplicates =
        duplicateCount > 5
          ? `${duplicateEmails.slice(0, 5).join(', ')} and ${duplicateCount - 5} more...`
          : duplicateEmails.join(', ');

      toast.error(`Found ${duplicateCount} duplicate email(s): ${displayDuplicates}`, {
        duration: 5000,
      });
    }

    if (validEmails.length > 0) {
      setUserEmails((prev) => [...prev, ...validEmails]);
      toast.success(`Successfully added ${validEmails.length} valid email(s)`);
    } else if (invalidEmails.length === 0 && duplicateEmails.length === 0) {
      toast.error('No valid emails found in the input');
    }
  };

  return { userEmails, parseEmailContent, setUserEmails };
};