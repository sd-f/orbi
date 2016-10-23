package foundation.softwaredesign.orbi.model.game.server;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class ServerInfo {

    private String version;
    private List<MessageOfTheDay> messages = new ArrayList<>();

    public String getVersion() {
        return version;
    }

    public void setVersion(String version) {
        this.version = version;
    }

    public List<MessageOfTheDay> getMessages() {
        return messages;
    }

    public void setMessages(List<MessageOfTheDay> messages) {
        this.messages = messages;
    }
}
