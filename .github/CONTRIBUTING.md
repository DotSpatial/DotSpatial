Contribute to DotSpatial
=====================

DotSpatial is driven by the community and contributors like you. We are excited that you're interested to help us move forward and improve DotSpatial.

The flow when contributing to DotSpatial is as follows:

1. Add issue (bug or new feature)
2. Wait for issue to be tagged `you-take-it`
3. Commit your changes and rebase
4. Create a pull request

Note that your contributions must be your own work and licensed under the same terms as DotSpatial.


## Reporting bugs

First, search the issue tracker to see if the bug is already there.
Please use the [discussion forums](http://dotspatial.codeplex.com/discussions) if you are unsure whether it is a bug in the library or a problem in your code.
When you are sure, please add defects in the issue tracker.

Please use markdown to format your code blocks with [syntax highlighting](https://help.github.com/articles/github-flavored-markdown/#syntax-highlighting) to make it easier for everyone to read. 

You could also use services like http://snipt.org or https://gist.github.com to share code snippets.

A bug report should include:

- used platform and tools version
- used DotSpatial version
- version of dependencies
- description of the issue
- sample code to reproduce the issue


## Suggesting new features

Please use the [discussion forums](http://dotspatial.codeplex.com/discussions) or add it directly in the issue tracker in the same way as bug reports.


## Create your own fork

1. Log in to GitHub and open the [DotSpatial](https://github.com/DotSpatial/DotSpatial/) origin repository. Click the "Fork" button to create your own fork of the repository.
2. Create a clone on your local system: `git clone https://github.com/yourusername/DotSpatial.git`


## Branch

You should base your work on the `master` branch.
Create a branch for the bugfix/feature you want to work on: `git branch bugfix-some-error`
Checkout the branch: `git checkout bugfix-some-error`

## Commit your changes

Before you start committing, please define your [full name](https://help.github.com/articles/setting-your-username-in-git/) and [e-mail](https://help.github.com/articles/setting-your-email-in-git/) address in git.

Please follow the style of the other messages in the commit history. Explain the intentions for the change and include the issue number. Include the issue number in the commit message, e.g. "#9945".

Please follow the coding-style:

- include XML comments for all methods, properties and fields
- use linq method syntax (not query syntax)
- keep it simple

To commit your changes, use the git command: `git commit -m <msg>`

When you are ready to make a pull request, please

- [Rebase](https://git-scm.com/docs/git-rebase) to the latest commit of the origin
- [Squash](http://gitready.com/advanced/2009/02/10/squashing-commits-with-rebase.html) each logical change to a single commit 

More info about rebasing

- https://git-scm.com/book/en/v2/Git-Branching-Rebasing
- https://blog.sourcetreeapp.com/2012/08/21/merge-or-rebase/
- http://gitready.com/advanced/2009/02/10/squashing-commits-with-rebase.html
- https://ariejan.net/2011/07/05/git-squash-your-latests-commits-into-one/
- https://git-scm.com/book/en/v2/Git-Tools-Rewriting-History
- https://www.atlassian.com/git/tutorials/rewriting-history/git-rebase-i

Finally, push your commits: `git push`


## Requirements for a pull request

- Include examples or unit tests for the change / new feature
- Update the CHANGELOG.md file
- Update the CONTRIBUTORS file if you are not already listed there
- Commits are rebased and squashed


## Creating a pull request

- Open the GitHub page for your fork and create a "Pull Request" (PR)
- Include the issue number in the pull request *comment* (not in the *title* where it will not be linked!)


## Merging

When the PR is submitted, Appveyor will build the code and report if the build was successful.
A team member will then review the PR, and merge the branch if everything looks OK.

If there are some issues with the PR, you must
- update your code and commit changes
- rebase and squash again
- force push: `git push -f`

The PR will be updated automatically when you push your changes.

## Common rules

To prevent the issue (backlog) list to grow indefinitely, an issue will be closed when you are not responding to requests by a project developer.
We need your help, so we will not close an issue by desire. But when you are not answering to a request by for a month we will close the issue.
If this happens in accident, feel free to re-open the issue...

The issue reporter can close the issue because:

- he/she verified the fix/change
- found out that it is not an DotSpatial issue but some problem in his/her code

In case the reporter does not respond to a request:

- a project developer is allowed to close the issue (after one month)

Someone can take over the issue from the reporter, when:

- he/she can reproduce the issue
- has some sample code
- has more time/information to assist in the resolution
- responds to requests by a project developer
