package foundation.softwaredesign.orbi.model;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.List;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class World {

    private List<GameObject> gameObjects;

    public List<GameObject> getGameObjects() {
        return gameObjects;
    }

    public void setGameObjects(List<GameObject> gameObjects) {
        this.gameObjects = gameObjects;
    }

}
