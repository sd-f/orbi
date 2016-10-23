package foundation.softwaredesign.orbi.model.game;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Statistics {

    private Long numberOfPlayers = new Long(0);
    private Long numberOfObjects = new Long(0);

    public Long getNumberOfPlayers() {
        return numberOfPlayers;
    }

    public void setNumberOfPlayers(Long numberOfPlayers) {
        this.numberOfPlayers = numberOfPlayers;
    }

    public Long getNumberOfObjects() {
        return numberOfObjects;
    }

    public void setNumberOfObjects(Long numberOfObjects) {
        this.numberOfObjects = numberOfObjects;
    }
}
