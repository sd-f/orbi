package foundation.softwaredesign.orbi.service;

import foundation.softwaredesign.orbi.rest.exception.VersionNotSupportedException;

import javax.enterprise.context.RequestScoped;
import javax.ws.rs.InternalServerErrorException;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class ServerService {

    private static Long SERVER_VERSION = new Long(3);

    public Long getVersion() {
        return SERVER_VERSION;
    }

    public void checkVersion(Long clientVersion) {
        if (!clientVersion.equals(SERVER_VERSION)) {
            throw new VersionNotSupportedException("Client version not matching, please update your app");
        }
    }
}
