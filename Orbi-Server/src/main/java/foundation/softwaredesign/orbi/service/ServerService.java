package foundation.softwaredesign.orbi.service;

import javax.enterprise.context.RequestScoped;
import javax.ws.rs.InternalServerErrorException;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class ServerService {

    private static Long SERVER_VERSION = new Long(1);

    public void checkVersion(Long clientVersion) {
        if (!clientVersion.equals(SERVER_VERSION)) {
            throw new InternalServerErrorException("Client version not matching, please update your app");
        }
    }
}
