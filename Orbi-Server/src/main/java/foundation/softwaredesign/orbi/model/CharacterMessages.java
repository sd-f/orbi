package foundation.softwaredesign.orbi.model;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.ArrayList;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class CharacterMessages {
    private List<CharacterMessage> messages = new ArrayList<>();

    public List<CharacterMessage> getMessages() {
        return messages;
    }

    public void setMessages(List<CharacterMessage> messages) {
        this.messages = messages;
    }
}
