package foundation.softwaredesign.orbi.model.game.gameobject;

import foundation.softwaredesign.orbi.model.game.transform.Transform;
import org.apache.commons.lang3.builder.ToStringBuilder;

import java.util.Date;

/**
 * @author Lucas Reeh <lr86gm@gmail.com>
 */
public class GameObject {

    private Long id;
    private Transform transform = new Transform();
    private String name;
    private String prefab;
    private Date createDate;
    private Long identityId;
    private String userText;
    private Integer constraints;

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public Long getId() {
        return id;
    }

    public void setId(Long id) {
        this.id = id;
    }

    public String getPrefab() {
        return prefab;
    }

    public void setPrefab(String prefab) {
        this.prefab = prefab;
    }

    @Override
    public String toString() {
        return ToStringBuilder.reflectionToString(this);
    }

    public Date getCreateDate() {
        return createDate;
    }

    public void setCreateDate(Date createDate) {
        this.createDate = createDate;
    }

    public Long getIdentityId() {
        return identityId;
    }

    public void setIdentityId(Long identityId) {
        this.identityId = identityId;
    }

    public Transform getTransform() {
        return transform;
    }

    public void setTransform(Transform transform) {
        this.transform = transform;
    }

    public String getUserText() {
        return userText;
    }

    public void setUserText(String userText) {
        this.userText = userText;
    }

    public Integer getConstraints() {
        return constraints;
    }

    public void setConstraints(Integer constraints) {
        this.constraints = constraints;
    }
}
