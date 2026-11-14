#include <_string.h>
#include <grp.h>
#include <stdlib.h>
#include <string.h>
#include <sys/types.h>
#include <pwd.h>
#include <unistd.h>

#include "groups.h"

void get_groups(const char *user, char **groups, int* ngroups) {
    struct passwd* info = getpwnam(user);

    int group_count = -1;
    getgrouplist(user, info->pw_gid, NULL, &group_count);

    int* group_ids = malloc(sizeof(int) * group_count);
    getgrouplist(user, info->pw_gid, group_ids, &group_count);

    *ngroups = group_count;
    groups = malloc(sizeof(char*) * *ngroups);

    for (int i = 0; i < *ngroups; ++i) {
        int gid = group_ids[i];

        struct group* group = getgrgid(gid);
        size_t len = strlen(group->gr_name);

        groups[i] = malloc(sizeof(char) * len);
        memcpy(groups[i], group->gr_name, len);
    }

    free(group_ids);
}
