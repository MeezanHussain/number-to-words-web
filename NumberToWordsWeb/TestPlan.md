# Test Plan – Number to Words Web Application

This test plan describes a series of test cases designed to verify
the correctness and robustness of the number‑to‑words conversion web
application. The tests cover typical use cases, boundary conditions
and error handling.

## 1. Functional tests

### 1.1 Basic conversions

| Test ID | Input amount | Expected output                                                             | Notes                         |
| ------- | ------------ | --------------------------------------------------------------------------- | ----------------------------- |
| TC1     | `0`          | `ZERO DOLLARS`                                                              | No cents should be appended.  |
| TC2     | `1`          | `ONE DOLLAR`                                                                | Singular form of “DOLLAR”.    |
| TC3     | `12`         | `TWELVE DOLLARS`                                                            | Teen value.                   |
| TC4     | `21`         | `TWENTY-ONE DOLLARS`                                                        | Hyphen between tens and ones. |
| TC5     | `105`        | `ONE HUNDRED AND FIVE DOLLARS`                                              | Tests “AND” within hundreds.  |
| TC6     | `569457`     | `FIVE HUNDRED AND SIXTY-NINE THOUSAND FOUR HUNDRED AND FIFTY-SEVEN DOLLARS` | Multi‑group number.           |
| TC7     | `1000000`    | `ONE MILLION DOLLARS`                                                       | Exact group name.             |
| TC8     | `123.45`     | `ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS`                 | Cents appended correctly.     |
| TC9     | `0.99`       | `ZERO DOLLARS AND NINETY-NINE CENTS`                                        | Zero dollars with cents.      |
| TC10    | `1.01`       | `ONE DOLLAR AND ONE CENT`                                                   | Singular “CENT”.              |

### 1.2 Large numbers

| Test ID | Input amount | Expected output |
| TC11 | `999999999999.99` | `NINE HUNDRED AND NINETY-NINE BILLION NINE HUNDRED AND NINETY-NINE MILLION NINE HUNDRED AND NINETY-NINE THOUSAND NINE HUNDRED AND NINETY-NINE DOLLARS AND NINETY-NINE CENTS` | Tests maximum supported value. |
| TC12 | `1001001001.01` | `ONE BILLION ONE MILLION ONE THOUSAND ONE DOLLAR AND ONE CENT` | Non‑trivial group combinations. |

### 1.3 Negative values

| Test ID | Input amount | Expected output                                                   | Notes                                  |
| ------- | ------------ | ----------------------------------------------------------------- | -------------------------------------- |
| TC13    | `-123.45`    | `MINUS ONE HUNDRED AND TWENTY-THREE DOLLARS AND FORTY-FIVE CENTS` | Negative values prefixed with “MINUS”. |

### 1.4 Invalid input

| Test ID | Input amount | Expected behaviour                                    | Notes                    |
| ------- | ------------ | ----------------------------------------------------- | ------------------------ |
| TC14    | Empty string | HTTP 400 error with message "Please enter an amount." | Blank input not allowed. |
| TC15    | `abc`        | HTTP 400 error with message "Invalid numeric amount." | Non‑numeric input.       |

## 2. Usability tests

1. UI clarity: Verify that the web page clearly instructs the user
   to enter an amount and provides immediate feedback. Ensure the
   placeholder (`"Enter amount e.g. 123.45"`) is visible.
2. Responsive design: Resize the browser window to various
   dimensions and confirm that the input field and button remain
   usable without overflowing or collapsing.
3. Error messages: Enter invalid data (e.g. letters, blank input)
   and check that the displayed error message is clear and does not
   leak server details.

## 4. Security considerations

1. Input validation: The server must validate the `amount` query
   parameter and refuse requests that cannot be parsed as a number.
2. Ensure that the application correctly encodes the response and does
   not echo untrusted input directly into HTML. Since the API returns plain
   text and the client inserts it as text content (not HTML), there
   is no injection risk.

## 5. Acceptance criteria

The application passes all functional tests, gracefully handles invalid
input and negative numbers, and provides a user‑friendly interface
according to the usability guidelines.
