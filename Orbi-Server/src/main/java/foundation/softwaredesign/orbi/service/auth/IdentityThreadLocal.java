package foundation.softwaredesign.orbi.service.auth;

import foundation.softwaredesign.orbi.persistence.entity.IdentityEntity;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class IdentityThreadLocal {

    public static final ThreadLocal userThreadLocal = new ThreadLocal();

    public static void set(IdentityEntity identity) {
        userThreadLocal.set(identity);
    }

    public static void unset() {
        userThreadLocal.remove();
    }

    public static IdentityEntity get() {
        return (IdentityEntity) userThreadLocal.get();
    }
}
