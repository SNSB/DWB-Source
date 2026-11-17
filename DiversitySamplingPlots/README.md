# DiversitySamplingPlots

# Git Branching Strategy & Development Workflow

This document describes the branching strategy and workflow for all shared repositories (DiversityWorkbench, DWBServices, GeoConversion, WpfControls, WpfSamplingPlots, etc.).

## Table of Contents

- [Branch Structure](#branch-structure)
- [Branch Naming Conventions](#branch-naming-conventions)
- [Development Workflow](#development-workflow)
- [Release Process](#release-process)
- [Hotfix Process](#hotfix-process)
- [Best Practices](#best-practices)
- [Examples](#examples)

## Branch Structure

```
main
│
├── develop
│   │
│   ├── dev_diversitysamplingplots
│   │   ├── dev_dsp_user1
│   │   ├── dev_dsp_user2
│   │   └── dev_dsp_feature-name
│   │
│   └── dev_diversityprojects
│       ├── dev_dp_user1
│       ├── dev_dp_user2
│       └── dev_dp_feature-name
```

### Branch Types

| Branch | Purpose | Protected | Lifetime |
|--------|---------|-----------|----------|
| main | Production-ready, released code | Yes | Permanent |
| develop | Integration/staging branch | Yes | Permanent |
| dev_diversitysamplingplots | DiversitySamplingPlots development | Recommended | Permanent |
| dev_diversityProjects | DiversityProjects development | Recommended | Permanent |
| dev_dsp_* | Personal/feature branches for dsp | No | Temporary |
| dev_dp_* | Personal/feature branches for DP | No | Temporary |

## Branch Naming Conventions

### Application Development Branches
- **DiversitySamplingPlots**: dev_diversitysamplingplots

### User/Feature Branches

Format: dev_app_username or dev_app_feature-description

**Examples:**
- dev_dsp_mm (maxmustermanns personal branch for DiversitySamplingPlots)
- dev_dsp_export-xml (Feature branch for xml export)
- dev_dsp_mm (maxmustermanns personal branch for DiversitySamplingPlots)

## Development Workflow

### 1. Starting New Work

**For DiversitySamplingPlots developers:**

Step 1: Ensure you have the latest code

git checkout dev_diversitysamplingplots
git pull origin dev_diversitysamplingplots

Step 2: Create your personal/feature branch

git checkout -b dev_dp_yourname dev_diversitysamplingplots

Step 3: Start coding!

### 2. dspily Development

Make changes and commit:

git add .
git commit -m "Clear description of changes"

Push your branch regularly:

git push origin dev_dsp_yourname

Sync with latest team changes (recommended dspily):

git checkout dev_diversitysamplingplots
git pull origin dev_diversitysamplingplots
git checkout dev_dsp_yourname
git merge dev_diversitysamplingplots

### 3. Completing Your Work

Step 1: Ensure your branch is up to dspte

git checkout dev_diversitysamplingplots
git pull origin dev_diversitysamplingplots
git checkout dev_dsp_yourname
git merge dev_diversitysamplingplots

Step 2: Resolve any conflicts if needed

Step 3: Test thoroughly

Step 4: Merge back to application branch

git checkout dev_diversitysamplingplots
git merge dev_dsp_yourname --no-ff

Step 5: Push

git push origin dev_diversitysamplingplots

Step 6: Delete your branch (optional, after confirmed working)

git branch -d dev_dsp_yourname
git push origin --delete dev_dsp_yourname

## Release Process

### DiversityProject Release

#### Phase 1: Team Integration

All DiversityProject developers merge their changes. Each developer:

git checkout dev_diversitysamplingplots
git pull origin dev_diversitysamplingplots
git merge dev_dsp_yourname --no-ff
git push origin dev_diversitysamplingplots

#### Phase 2: Pre-Release Integration

Release manager merges to develop for integration testing:

git checkout develop
git pull origin develop
git merge dev_diversitysamplingplots --no-ff -m "Merge dsp for release v1.X.X"
git push origin develop

Notify team that develop is ready for testing.

#### Phase 3: Testing & Verification
- Run all tests
- Perform integration testing with DiversitySamplingPlots solution

#### Phase 4: Release to Main

Release manager executes:

Step 1: Merge to main

git checkout main
git pull origin main
git merge develop --no-ff -m "Release v1.X.X for DiversitySamplingPlots"

Step 2: Tag the release

git tag -a v1.X.X-dsp -m "DiversitySamplingPlots Release v1.X.X"

Step 3: Push

git push origin main --tags

Step 4: Sync back to development branches

git checkout dev_diversitysamplingplots
git merge main
git push origin dev_diversitysamplingplots

git checkout develop
git merge main
git push origin develop

## Hotfix Process

For critical bugs in production (main branch):

Step 1: Create hotfix branch from main

git checkout main
git pull origin main
git checkout -b hotfix/critical-bug-description

Step 2: Fix the bug

git add .
git commit -m "Fix: critical bug description"

Step 3: Merge to main

git checkout main
git merge hotfix/critical-bug-description --no-ff
git tag -a v1.X.Y-dsp -m "Hotfix: bug description"
git push origin main --tags

Step 4: Merge to develop

git checkout develop
git merge main
git push origin develop

Step 5: Merge to application branches

git checkout dev_diversitysamplingplots
git merge main
git push origin dev_diversitysamplingplots

git checkout dev_diversitysamplingplots
git merge main
git push origin dev_diversitysamplingplots

Step 6: Delete hotfix branch

git branch -d hotfix/critical-bug-description

## Best Practices

### Branch Management
- Keep personal branches short-lived (delete after merging)
- Sync with application branch frequently
- Don't work directly on dev_diversitysamplingplots or dev_diversitycollectiions (use personal branches)
- Never commit directly to main or develop

## Example: Full Release Cycle

DiversitySamplingPlots v2.5.0 Release

Step 1: All developers merge their dev_dsp_* branches to dev_diversitysamplingplots

Step 2: Release manager merges to develop

git checkout develop
git merge dev_diversitysamplingplots --no-ff -m "Merge dsp for release v2.5.0"
git push origin develop

Step 3: Testing phase (1-2 days)
QA team tests, bugs are fixed on dev_diversitysamplingplots and re-merged

Step 4: Release

git checkout main
git merge develop --no-ff -m "Release v2.5.0 for DiversitySamplingPlots"
git tag -a v2.5.0-dsp -m "DiversitySamplingPlots Release v2.5.0 - Added Excel export, improved UI"
git push origin main --tags

Step 5: Sync back

git checkout dev_diversitysamplingplots
git merge main
git push origin dev_diversitysamplingplots

git checkout develop
git merge main
git push origin develop

---

**Last Updspted:** 2026-10-06
**Document Version:** 1.0
