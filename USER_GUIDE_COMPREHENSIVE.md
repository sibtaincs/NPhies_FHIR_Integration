# ?? **NPHIES FHIR INTEGRATION - COMPLETE USER GUIDE**

## Table of Contents
1. [Introduction](#introduction)
2. [Getting Started](#getting-started)
3. [User Interface Navigation](#user-interface-navigation)
4. [Submitting Claims](#submitting-claims)
5. [Eligibility Verification](#eligibility-verification)
6. [Pre-Authorization](#pre-authorization)
7. [Tracking Claims](#tracking-claims)
8. [Managing Corrections](#managing-corrections)
9. [Appeals Process](#appeals-process)
10. [Reports & Analytics](#reports--analytics)
11. [Troubleshooting](#troubleshooting)
12. [FAQs](#faqs)

---

## Introduction

### What is NPHIES FHIR Integration?

The NPHIES FHIR Integration System is a comprehensive healthcare claims processing platform that:

- **Streamlines Claim Submission** - Submit claims directly to NPHIES
- **Verifies Eligibility** - Check member coverage in real-time
- **Manages Pre-Authorizations** - Submit and track pre-auth requests
- **Provides Real-time Status** - Track claim status end-to-end
- **Handles Corrections** - Manage claim amendments easily
- **Processes Appeals** - Submit and track appeals for denied claims
- **Offers Analytics** - Detailed reports and performance metrics

### Key Features

? **Real-time Eligibility Verification**  
? **Batch Claim Submission**  
? **Automated Pre-Authorization**  
? **Instant Status Tracking**  
? **Error Detection & Correction**  
? **Appeals Management**  
? **Comprehensive Reporting**  
? **24/7 Monitoring**  

---

## Getting Started

### System Requirements

**Browser Requirements:**
- Chrome 90+
- Firefox 88+
- Safari 14+
- Edge 90+

**Internet:**
- Broadband connection (5 Mbps minimum)
- Stable connection for optimal performance

### Login & Authentication

1. **Go to Login Page**
   - Navigate to: https://nphies-fhir.health/login

2. **Enter Credentials**
   - Username: Your provider ID
   - Password: Your secure password

3. **Two-Factor Authentication** (if enabled)
   - Enter code from authenticator app
   - Or click "Send code via email"

4. **Accept Terms**
   - Review and accept terms of service
   - Click "I Agree"

5. **Access Dashboard**
   - You'll be directed to your dashboard

### First Time Setup

**Step 1: Profile Configuration**
- Click "Settings" ? "Profile"
- Verify provider information
- Update contact details
- Set notification preferences

**Step 2: Team Management**
- Go to "Team" ? "Add Member"
- Enter team member details
- Set user role (Admin, Operator, Viewer)
- Send invitation link

**Step 3: Organization Settings**
- Configure default values
- Set batch processing preferences
- Enable/disable features
- Configure email notifications

---

## User Interface Navigation

### Dashboard Overview

The Dashboard shows:

```
???????????????????????????????????????????????????
? NPHIES FHIR Integration Dashboard  ?
??????????????????????????????????????????????????
? Claims   ? Eligibility? Pre-Auth    ? Analytics?
? (125)    ? (98 due)   ? (45 pending)?         ?
??????????????????????????????????????????????????
?     ?
? Recent Activity      ?
? ?????????????????????????????????????????????   ?
? • Claim CLM-001: Approved ($1,200)  ? Today   ?
? • Claim CLM-002: Needs Correction   ?? Today   ?
? • Pre-Auth: PAU-001 Approved ? Yesterday?
?       ?
????????????????????????????????????????????????????
? Quick Stats              ?
? ?????????????????????????????????????????????    ?
? Submitted: 250 | Approved: 200 | Denied: 30    ?
? Success Rate: 96.2% | Avg Processing: 2 days   ?
????????????????????????????????????????????????????
```

### Main Menu

| Option | Description |
|--------|-------------|
| **Dashboard** | Home page with overview |
| **Claims** | Submit and manage claims |
| **Eligibility** | Check member coverage |
| **Pre-Auth** | Request pre-authorization |
| **Tracking** | Monitor claim status |
| **Corrections** | Amend submitted claims |
| **Appeals** | Manage denied claim appeals |
| **Reports** | View analytics and reports |
| **Settings** | Configure preferences |
| **Support** | Help and documentation |

### Navigation Tips

- **Quick Search**: Use search box to find claims by ID
- **Filters**: Apply filters to narrow results
- **Export**: Export data to CSV/PDF
- **Favorites**: Star frequently used pages
- **Shortcuts**: Use keyboard shortcuts (? for help)

---

## Submitting Claims

### Step-by-Step Claim Submission

#### Step 1: Start New Claim

1. Click **"Claims"** in main menu
2. Click **"+ Submit New Claim"** button
3. Select claim type:
   - Medical
   - Dental
   - Pharmacy
4. Click **"Continue"**

#### Step 2: Patient Information

```
Patient Details
?????????????????????????????????
Member ID:        [SUB001            ]  ?
First Name:       [John         ]
Last Name:        [Smith        ]
DOB:   [01/15/1980      ]
Gender:           [Male        ?]
?????????????????????????????????
```

- Fill in patient details
- Verify against NPHIES database (auto-populated)
- Correct any mismatches
- Click **"Next"**

#### Step 3: Service Information

```
Service Details
?????????????????????????????????
Service Type:     [Office Visit      ?]
Service Code:     [99213     ]
Service Date:     [01/15/2024        ]
Amount:           [150.00 ]
Provider NPI:     [1234567890        ]
?????????????????????????????????
```

- Select service type
- Enter service code
- Enter service date
- Enter amount
- Click **"Next"**

#### Step 4: Diagnosis & Medical Details

```
Diagnosis Information
?????????????????????????????????
Primary Diagnosis: [J06.9       ] (Acute URI)
Secondary:  [M79.3    ] (Myalgia)
Primary:           [99213       ] (Office Visit)
Secondary:         [71045       ] (Shoulder X-ray)
?????????????????????????????????
```

- Enter diagnosis codes (ICD-10)
- Select primary diagnosis
- Add secondary diagnoses
- Add service procedures
- Click **"Next"**

#### Step 5: Insurance Information

```
Insurance Details
?????????????????????????????????
Insurance ID:     [INS-001     ]
Insurer:        [ABC Insurance]
Group Number:     [GRP-001           ]
Plan:             [Standard Plan     ]
Coverage Status:  [Active       ?]
?????????????????????????????????
```

- Verify insurance information
- Update if necessary
- Confirm coverage status
- Click **"Next"**

#### Step 6: Attachments (Optional)

```
Attachments
?????????????????????????????????
?? Supporting Documents
  ? Medical Records
? X-ray Images
  ? Lab Results
  ? Prescription
?????????????????????????????????
```

- Attach supporting documents
- Upload lab results if required
- Include clinical notes
- Click **"Next"**

#### Step 7: Review & Submit

```
Claim Review
?????????????????????????????????
Patient:          John Smith (SUB001)
Service: Office Visit ($150)
Diagnosis:        J06.9 (Acute URI)
Insurer:          ABC Insurance
Status:           Ready to Submit ?
?????????????????????????????????
```

- Review all information
- Make any corrections
- Click **"Submit Claim"**
- Confirmation message appears

### Batch Submission

For submitting multiple claims:

1. Click **"Batch Submit"**
2. Upload CSV/Excel file
3. Map columns to fields
4. Preview data
5. Click **"Submit Batch"**
6. Monitor batch progress

**Batch Format (CSV)**:
```csv
MemberId,FirstName,LastName,ServiceCode,ServiceDate,Amount,DiagnosisCode
SUB001,John,Smith,99213,01/15/2024,150,J06.9
SUB002,Jane,Doe,99214,01/15/2024,200,E11.9
```

---

## Eligibility Verification

### Check Member Eligibility

#### Method 1: Quick Check

1. Click **"Eligibility"** ? **"Quick Check"**
2. Enter **Member ID**: `SUB001`
3. Click **"Check"**
4. View eligibility status:

```
Member Eligibility
?????????????????????????????????
Status:    Active ?
Plan:     Standard Plan
Group:      GRP-001
Coverage:         Effective
Deductible:       $1,000 (Met: $500)
Out-of-Pocket:    $3,000 (Met: $1,200)
?????????????????????????????????
```

#### Method 2: Detailed Check

1. Click **"Eligibility"** ? **"Detailed Check"**
2. Fill in all details:
   - Member ID
   - Service Date
   - Service Code
3. Click **"Verify"**
4. Review benefit details:

```
Benefit Details for Service Code 99213
?????????????????????????????????
Coverage: 80% (Plan covers 80%)
Copay:            $25
Deductible:     Applies
Auth Required: No
Prior Auth:       N/A
?????????????????????????????????
```

### Understanding Eligibility Status

| Status | Meaning | Action |
|--------|---------|--------|
| **Active** | Member is eligible | Submit claim |
| **Inactive** | Member not covered | Contact insurer |
| **Suspended** | Temporarily inactive | Investigate |
| **Terminated** | Coverage ended | Contact member |
| **Pending** | Awaiting verification | Wait for update |

### Eligibility Insights

- **Real-time Updates**: Checked directly with NPHIES
- **Auto-refresh**: Updates every 24 hours
- **Caching**: Faster subsequent checks
- **Alerts**: Notified of coverage changes

---

## Pre-Authorization

### Request Pre-Authorization

#### Step 1: Start Request

1. Click **"Pre-Auth"** ? **"+ New Request"**
2. Enter patient details
3. Click **"Continue"**

#### Step 2: Service Details

```
Pre-Authorization Details
?????????????????????????????????
Service Type:   [Specialty Exam    ?]
Service Code:     [92014             ]
Estimated Cost:   [$500              ]
Clinical Reason:  [Comprehensive eye exam]
?????????????????????????????????
```

#### Step 3: Medical Justification

```
Clinical Information
?????????????????????????????????
Diagnosis:        [H44.003 (Retinal disease)]
Justification:    [Frequent headaches, 
         visual disturbances,
     need comprehensive eye exam]
?????????????????????????????????
```

- Explain medical necessity
- Include clinical findings
- Attach supporting documents
- Click **"Submit"**

#### Step 4: Confirmation

```
Pre-Auth Request Submitted
?????????????????????????????????
Authorization ID: AUTH-2024-001
Status: Submitted ?
Submitted Date:   01/15/2024
Estimated Decision: 24 hours
?????????????????????????????????
```

### Monitor Pre-Auth Status

1. Click **"Pre-Auth"** ? **"My Requests"**
2. View request status
3. Check decision
4. Link to claim submission

**Status Timeline**:
- Submitted ? Accepted ? Reviewing ? Decided ? Approved/Denied

---

## Tracking Claims

### View Claim Status

#### Method 1: Dashboard

Status indicators show on dashboard:
- ?? **Approved** - Claim approved
- ?? **Processing** - Under review
- ?? **Denied** - Claim denied
- ?? **Correction Needed** - Needs amendment

#### Method 2: Search Claims

1. Click **"Tracking"** ? **"Search Claims"**
2. Enter **Claim ID**: `CLM-001`
3. Click **"Search"**
4. View full claim details

#### Method 3: Filter & Sort

```
Filters Available:
  ? Status (All, Submitted, Approved, Denied)
  ? Date Range
  ? Amount Range
  ? Provider
  ? Insurance

Sort By:
  • Date (Newest/Oldest)
  • Amount (High/Low)
  • Status
```

### Claim Detail View

```
Claim Details - CLM-001
?????????????????????????????????
Patient:          John Smith (SUB001)
Service Date:     01/15/2024
Service:          Office Visit (99213)
Diagnosis:        J06.9 (Acute URI)
Amount:           $150.00

Status:           APPROVED ? (Jan 16)
Approved Amount:  $120.00
Patient Resp:   $30.00 (Copay)
Payment Status:   PAID (Jan 20)
Payment Amount:   $120.00

Insurance:        ABC Insurance
Authorization:    PRE-001 (Valid)
Attachments:      2 files
?????????????????????????????????
```

### Status Explanations

| Status | Timeline | Meaning |
|--------|----------|---------|
| **Submitted** | Day 1 | Received by NPHIES |
| **Accepted** | Day 1-2 | Format validation passed |
| **Processing** | Day 2-3 | Under clinical review |
| **Approved** | Day 3-5 | Approved for payment |
| **Denied** | Day 3-5 | Not covered/eligible |
| **Paid** | Day 5-10 | Payment issued |

---

## Managing Corrections

### Submit Claim Correction

When a claim needs correction:

1. Click **"Corrections"** ? **"+ New Correction"**
2. Select claim to correct
3. Choose fields to correct:

```
Fields to Correct
?????????????????????????????????
? Patient Information
? Service Details
? Diagnosis Codes
? Insurance Information
? Amount
? Attachments
?????????????????????????????????
```

4. Enter corrections
5. Explain reason for correction
6. Submit correction

### Correction Status

```
Correction #CORR-001
?????????????????????????????????
Original Claim:   CLM-001
Correction Date:  01/16/2024
Status:  Accepted ?
Changes Made:     • Amount: $150 ? $160
New Claim ID:     CLM-001R1
Resubmitted:      01/16/2024
?????????????????????????????????
```

---

## Appeals Process

### Submit Appeal

For denied claims:

1. Click **"Appeals"** ? **"+ New Appeal"**
2. Select claim to appeal
3. Review denial reason
4. Provide appeal justification:

```
Appeal Details
?????????????????????????????????
Claim ID:         CLM-001
Denial Reason:    Exceeds benefit limit
Appeal Reason:    [Text area for justification]
Supporting Docs:  [Upload files]
Contact Info:     [Phone/Email]
?????????????????????????????????
```

5. Attach supporting documents
6. Submit appeal
7. Get appeal ID and deadline

### Appeal Timeline

```
Appeal Status - APP-001
?????????????????????????????????
Submitted:        01/16/2024
Deadline:         02/15/2024 (30 days)
Status:Under Review ?
Expected Decision: 01/30/2024
?????????????????????????????????
```

---

## Reports & Analytics

### Dashboard Analytics

View key metrics:

```
Performance Metrics
?????????????????????????????????
Total Claims:     500
Approved:         450 (90%)
Denied:           30 (6%)
Corrections:      20 (4%)

Avg Processing:   2.5 days
Success Rate:     96.2%
Payment Avg:      $180.50
?????????????????????????????????
```

### Generate Reports

1. Click **"Reports"** ? **"Generate Report"**
2. Select report type:
   - Claims Summary
   - Financial Report
   - Performance Report
   - Compliance Report
3. Set date range
4. Click **"Generate"**
5. Download PDF/Excel

### Export Data

```
Export Options:
?????????????????????????????????
Format:    [PDF ?] [Excel ?] [CSV ?]
Date:      [From] [To]
Filter:    [Status] [Provider]
Include:   [?] All Details
 [?] Attachments
?????????????????????????????????
```

---

## Troubleshooting

### Common Issues & Solutions

**Issue**: "Claim Submission Failed"
- **Cause**: Invalid data format
- **Solution**: 
  1. Review error message
  2. Correct highlighted fields
  3. Re-submit claim

**Issue**: "Member Not Found"
- **Cause**: Incorrect member ID
- **Solution**:
  1. Verify member ID with patient
  2. Check member ID format
  3. Search by name instead

**Issue**: "Service Not Covered"
- **Cause**: Service not included in plan
- **Solution**:
  1. Check plan details
  2. Verify service code
  3. Contact insurer for coverage details

**Issue**: "Authorization Required"
- **Cause**: Service requires pre-auth
- **Solution**:
  1. Submit pre-authorization request
  2. Wait for approval
  3. Submit claim after approval

**Issue**: "System Timeout"
- **Cause**: Network or server issue
- **Solution**:
  1. Wait a few minutes
  2. Refresh page
  3. Try again
  4. Contact support if issue persists

### Getting Help

**In-app Support**:
- Click "?" in top-right corner
- View help articles
- Live chat with support (if available)

**Contact Support**:
- Email: support@nphies-fhir.health
- Phone: 1-800-NPHIES-1
- Web: https://support.nphies-fhir.health

---

## FAQs

**Q: How long does claim processing take?**
A: Average 2-3 days for NPHIES review. Once approved, payment typically follows within 5-10 days.

**Q: Can I submit claims in bulk?**
A: Yes! Use the Batch Submit feature to upload CSV files with multiple claims.

**Q: What if my claim is denied?**
A: You can request a correction (if data was wrong) or file an appeal (if you disagree with decision).

**Q: Is my data secure?**
A: Yes, all data is encrypted (HTTPS/TLS), and the system meets HIPAA and NPHIES security requirements.

**Q: Can I access from mobile?**
A: Yes, the system is fully responsive and works on smartphones and tablets.

**Q: What are the system hours?**
A: The system is available 24/7, but NPHIES processing is during standard business hours.

**Q: How do I reset my password?**
A: Click "Forgot Password" on login page, enter your email, and follow the link sent to you.

**Q: Can multiple users access the same account?**
A: Yes, add team members in Settings ? Team Management to grant them access.

**Q: What file formats are accepted for attachments?**
A: PDF, JPG, PNG, TIFF (up to 10 MB per file).

---

## Tips & Best Practices

### ? Do's

- ? Always verify patient information before submitting
- ? Check eligibility before claim submission
- ? Include necessary documentation
- ? Keep security credentials safe
- ? Monitor claim status regularly
- ? Submit corrections immediately when needed

### ? Don'ts

- ? Don't submit duplicate claims
- ? Don't enter fake information
- ? Don't share login credentials
- ? Don't submit without verification
- ? Don't ignore denial notifications
- ? Don't miss appeal deadlines

---

**Document**: USER_GUIDE.md  
**Version**: 2.0  
**Last Updated**: January 2024  
**Status**: Complete  

