package foundation.softwaredesign.orbi.model.game.gameobject.ai;

import foundation.softwaredesign.orbi.model.game.transform.Transform;

import javax.xml.bind.annotation.XmlRootElement;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class AiProperties {
    private Transform target = new Transform();

    private String lastTargetUpdate;

    public Transform getTarget() {
        return target;
    }

    public void setTarget(Transform target) {
        this.target = target;
    }

    public String getLastTargetUpdate() {
        return lastTargetUpdate;
    }

    public void setLastTargetUpdate(String lastTargetUpdate) {
        this.lastTargetUpdate = lastTargetUpdate;
    }
}
