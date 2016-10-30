package foundation.softwaredesign.orbi.model.game.gameobject.ai;

import foundation.softwaredesign.orbi.model.game.transform.Transform;

import javax.xml.bind.annotation.XmlRootElement;
import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
@XmlRootElement
public class AiProperties {
    private Transform target = new Transform();
    private Date lastTargetUpdate;

    public Transform getTarget() {
        return target;
    }

    public void setTarget(Transform target) {
        this.target = target;
    }

    public Date getLastTargetUpdate() {
        return lastTargetUpdate;
    }

    public void setLastTargetUpdate(Date lastTargetUpdate) {
        this.lastTargetUpdate = lastTargetUpdate;
    }
}
