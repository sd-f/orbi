package foundation.softwaredesign.orbi.model;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class Player {
    private GeoPosition geoPosition;
    private GameObject gameObjectToCraft;
    private Long selectedObjectId;
    private Long clientVersion;

    public GameObject getGameObjectToCraft() {
        return gameObjectToCraft;
    }

    public void setGameObjectToCraft(GameObject gameObjectToCraft) {
        this.gameObjectToCraft = gameObjectToCraft;
    }

    public GeoPosition getGeoPosition() {
        return geoPosition;
    }

    public void setGeoPosition(GeoPosition geoPosition) {
        this.geoPosition = geoPosition;
    }

    public Long getSelectedObjectId() {
        return selectedObjectId;
    }

    public void setSelectedObjectId(Long selectedObjectId) {
        this.selectedObjectId = selectedObjectId;
    }

    public Long getClientVersion() {
        return clientVersion;
    }

    public void setClientVersion(Long clientVersion) {
        this.clientVersion = clientVersion;
    }
}
