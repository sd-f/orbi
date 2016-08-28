package foundation.softwaredesign.orbi.service.authorization;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class TokenThreadLocal {

    public static final ThreadLocal userThreadLocal = new ThreadLocal();

    public static void set(String token) {
        userThreadLocal.set(token);
    }

    public static void unset() {
        userThreadLocal.remove();
    }

    public static String get() {
        return (String) userThreadLocal.get();
    }
}
