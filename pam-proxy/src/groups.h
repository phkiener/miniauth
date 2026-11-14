#pragma once

/*
 * Determine the groups of a user.
 *
 * @param user the user whose groups to determine
 * @param groups location where the list of groups was written to; needs to be freed by the caller (both the list and each entry)
 * @param ngroups number of groups in groups
 */
void get_groups(const char* user, char** groups, int* ngroups);
