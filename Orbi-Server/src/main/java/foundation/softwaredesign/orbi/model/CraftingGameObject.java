package foundation.softwaredesign.orbi.model;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class CraftingGameObject {
    private GeoPosition playerGeoPosition;
    private GameObject gameObject;

    public GeoPosition getPlayerGeoPosition() {
        return playerGeoPosition;
    }

    public void setPlayerGeoPosition(GeoPosition playerGeoPosition) {
        this.playerGeoPosition = playerGeoPosition;
    }

    public GameObject getGameObject() {
        return gameObject;
    }

    public void setGameObject(GameObject gameObject) {
        this.gameObject = gameObject;
    }
}
