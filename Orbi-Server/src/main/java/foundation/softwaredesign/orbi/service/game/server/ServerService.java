package foundation.softwaredesign.orbi.service.game.server;

import foundation.softwaredesign.orbi.model.game.server.ServerInfo;
import foundation.softwaredesign.orbi.rest.exception.VersionNotSupportedException;

import javax.enterprise.context.RequestScoped;
import javax.inject.Inject;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@RequestScoped
public class ServerService {

    @Inject
    MessageOfTheDayService messageOfTheDayService;

    private static Long SERVER_VERSION = new Long(23);

    public Long getVersion() {
        return SERVER_VERSION;
    }

    public void checkVersion(Long clientVersion) {
        if (!clientVersion.equals(SERVER_VERSION)) {
            throw new VersionNotSupportedException("Client version does not match, please update your app");
        }
    }

    public ServerInfo loadInfo() {
        ServerInfo info = new ServerInfo();
        info.setVersion(SERVER_VERSION.toString());
        info.setMessages(messageOfTheDayService.loadLatestMessage());
        return info;
    }
}
